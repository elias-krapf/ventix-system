using System.Xml.Serialization;

namespace VentixSystem.System.Model.Chat
{
    public class ChatFormat
    {
        
        [XmlAttribute("permission")]
        public string Permission { get; set; }
        public string Format { get; set; }

        public bool UseRichText { get; set; }
        
    }
}