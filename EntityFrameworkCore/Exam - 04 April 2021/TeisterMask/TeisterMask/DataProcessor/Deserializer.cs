namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportProjectDTO[]), new XmlRootAttribute("Projects"));

            var reader = new StringReader(xmlString);

            var xmlProjects = (ImportProjectDTO[])serializer.Deserialize(reader);

            reader.Close();

            foreach (var project in xmlProjects)
            {
                if (!IsValid(project))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project currentProject = new Project
                {
                    Name = project.Name,
                    OpenDate = DateTime.ParseExact(project.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                };

                if (string.IsNullOrWhiteSpace(project.DueDate))
                {
                    currentProject.DueDate = null;
                }
                else
                {
                    currentProject.DueDate = DateTime.ParseExact(project.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                foreach (var task in project.Tasks)
                {
                    if (!IsValid(task) ||
                        DateTime.ParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < currentProject.OpenDate ||
                        DateTime.ParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > currentProject.DueDate ||
                        !Enum.IsDefined(typeof(ExecutionType), task.ExecutionType) ||
                        !Enum.IsDefined(typeof(LabelType), task.LabelType))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    currentProject.Tasks.Add(new Task
                    {
                        Name = task.Name,
                        OpenDate = DateTime.ParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)task.ExecutionType,
                        LabelType = (LabelType)task.LabelType
                    });
                }

                context.Projects.Add(currentProject);

                sb.AppendLine(string.Format(SuccessfullyImportedProject, currentProject.Name, currentProject.Tasks.Count));
            }

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var jsonEmployees = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString);

            foreach (var employee in jsonEmployees)
            {
                if (!IsValid(employee))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee currentEmployee = new Employee
                {
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.Phone
                };

                foreach (var task in employee.Tasks.Distinct())
                {
                    if (!context.Tasks.Any(x => x.Id == task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    currentEmployee.EmployeesTasks.Add(new EmployeeTask { TaskId = task });
                }

                context.Employees.Add(currentEmployee);

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, currentEmployee.Username, currentEmployee.EmployeesTasks.Count));
            }

            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}