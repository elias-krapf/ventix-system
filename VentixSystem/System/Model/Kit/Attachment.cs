using System.Xml.Serialization;

namespace VentixSystem.System.Model.Kit
{
    public class Attachment
    {
        [XmlArrayItem("attachment")]
        public ushort[] Attachments { get; set; }
    }
}