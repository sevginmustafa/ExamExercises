namespace TeisterMask.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var serializer = new XmlSerializer(typeof(ExportProjectsWithTasksDTO[]), new XmlRootAttribute("Projects"));

            var projects = context.Projects
                .ToArray()
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectsWithTasksDTO
                {
                    TasksCount = p.Tasks.Count,
                    Name = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new ExportTasksForProjectsDTO
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.Name)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            serializer.Serialize(writer, projects, namespaces);

            writer.Close();

            return writer.ToString();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .OrderByDescending(e => e.EmployeesTasks.Count(et => et.Task.OpenDate >= date))
                .Take(10)
                .Select(e => new ExportMostBusiestEmployeesDTO
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et=>et.Task.Name)
                    .Select(et => new ExportTasksDTO
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("MM/dd/yyyy"),
                        DueDate = et.Task.DueDate.ToString("MM/dd/yyyy"),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    .ToArray()
                })
                .ToArray()
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username);

            var jsonEmployees = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return jsonEmployees;
        }
    }
}