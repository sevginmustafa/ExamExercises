namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportPrisonersWitthCellsAndOfficerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CellNumber { get; set; }

        public ExportOfficersDTO[] Officers { get; set; }

        public double TotalOfficerSalary { get; set; }
    }
}
