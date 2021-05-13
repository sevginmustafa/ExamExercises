using System;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z][A-Za-z]+$")]
        public string Nickname { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ImportMailDTO[] Mails { get; set; }
    }
}
