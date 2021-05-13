using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        public ImportCellDTO[] Cells { get; set; }
    }
}
