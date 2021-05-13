using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class ImportPurchaseDTO
    {
        [Required]
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [Required]
        [XmlElement("Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}$")]
        [XmlElement("Key")]
        public string ProductKey { get; set; }

        [Required]
        [XmlElement("Card")]
        public string Card { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
