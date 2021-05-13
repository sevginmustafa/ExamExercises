﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBookDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [XmlElement("Genre")]
        public int Genre { get; set; }

        [XmlElement("Price")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [XmlElement("Pages")]
        [Range(50, 5000)]
        public int Pages { get; set; }

        [XmlElement("PublishedOn")]
        [Required]
        public string PublishedOn { get; set; }
    }
}
