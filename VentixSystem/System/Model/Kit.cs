using System.Xml.Serialization;

namespace VentixSystem.System.Model
{
    public class Kit
    {
        public string Name { get; set; }
        
        [XmlArrayItem("itemId")]
        public ushort[] Items { get; set; }
    }
}