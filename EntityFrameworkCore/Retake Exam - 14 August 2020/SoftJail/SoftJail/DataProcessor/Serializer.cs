namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(x => ids.Contains(x.Id))
                .Select(x => new ExportPrisonersWitthCellsAndOfficerDTO
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(po => new ExportOfficersDTO
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = double.Parse(x.PrisonerOfficers.Sum(o => o.Officer.Salary).ToString("f2"))
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            var jsonPrisoners = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonPrisoners;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var prisoners = context.Prisoners
                .Where(x => prisonersNames.Contains(x.FullName))
                .Select(x => new ExportPrisonersWithMailsDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    Mails = x.Mails.Select(x => new ExportMailsDTO
                    {
                        Description = new string(x.Description.Reverse().ToArray())
                    })
                    .ToArray()
                })
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.Id)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportPrisonersWithMailsDTO[]), new XmlRootAttribute("Prisoners"));

            var writer = new StringWriter();

            serializer.Serialize(writer, prisoners, namespaces);

            writer.Close();

            return writer.ToString();
        }
    }
}