using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDTO
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        public bool HasWindow { get; set; }
    }
}
