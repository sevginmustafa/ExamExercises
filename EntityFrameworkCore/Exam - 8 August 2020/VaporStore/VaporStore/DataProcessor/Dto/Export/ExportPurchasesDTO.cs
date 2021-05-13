using System;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchasesDTO
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        public string Date { get; set; }

        public ExportUserGamesDTO Game { get; set; }
    }
}
