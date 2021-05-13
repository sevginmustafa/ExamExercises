namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var jsonDepartments = JsonConvert.DeserializeObject<ImportDepartmentDTO[]>(jsonString);

            foreach (var item in jsonDepartments)
            {
                if (!IsValid(item) || item.Cells.Length == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department
                {
                    Name = item.Name,
                };

                foreach (var cell in item.Cells)
                {
                    if (!IsValid(cell))
                    {
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    department.Cells.Add(new Cell
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    });
                }

                if (department.Cells.Count == 0)
                {
                    continue;
                }

                context.Departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var jsonPrisoners = JsonConvert.DeserializeObject<ImportPrisonerDTO[]>(jsonString);

            foreach (var item in jsonPrisoners)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Prisoner prisoner = null;

                if (item.ReleaseDate == null)
                {
                    prisoner = new Prisoner
                    {
                        FullName = item.FullName,
                        Nickname = item.Nickname,
                        Age = item.Age,
                        IncarcerationDate = DateTime.ParseExact(item.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ReleaseDate = null,
                        Bail = item.Bail,
                        CellId = item.CellId
                    };
                }
                else
                {
                    prisoner = new Prisoner
                    {
                        FullName = item.FullName,
                        Nickname = item.Nickname,
                        Age = item.Age,
                        IncarcerationDate = DateTime.ParseExact(item.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ReleaseDate = DateTime.ParseExact(item.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Bail = item.Bail,
                        CellId = item.CellId
                    };
                }

                foreach (var mail in item.Mails)
                {
                    if (!IsValid(mail))
                    {
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    prisoner.Mails.Add(new Mail
                    {
                        Description = mail.Description,
                        Sender = mail.Sender,
                        Address = mail.Address
                    });
                }

                if (prisoner.Mails.Count == 0)
                {
                    continue;
                }

                context.Prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportOfficerDTO[]), new XmlRootAttribute("Officers"));

            var reader = new StringReader(xmlString);

            var xmlOfficers = (ImportOfficerDTO[])serializer.Deserialize(reader);

            reader.Close();

            foreach (var item in xmlOfficers)
            {
                if (!IsValid(item) ||
                    !Enum.TryParse(item.Position, out Position position) ||
                    !Enum.TryParse(item.Weapon, out Weapon weapon))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer
                {
                    FullName = item.FullName,
                    Salary = item.Salary,
                    Position = (Position)Enum.Parse(typeof(Position), item.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), item.Weapon),
                    DepartmentId = item.DepartmentId
                };

                foreach (var prisoner in item.Prisoners.Select(x => x.Id).Distinct())
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner { PrisonerId = prisoner });
                }

                context.Officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}