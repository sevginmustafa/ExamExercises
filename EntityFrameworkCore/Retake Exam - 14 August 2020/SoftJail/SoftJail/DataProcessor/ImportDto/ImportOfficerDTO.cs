using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Range(0.0, double.MaxValue)]
        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public  ImportPriconersOfficerDTO[] Prisoners { get; set; }
    }
}
