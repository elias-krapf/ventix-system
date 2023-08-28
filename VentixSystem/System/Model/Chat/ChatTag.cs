using System;
using System.Xml.Serialization;

namespace VentixSystem.System.Model.Chat
{
        [Serializable]
        public class ChatTag
        {
            [XmlAttribute("permission")]
            public string Permission { get; set; }
            public string Prefix { get; set; }
            public string Suffix { get; set; }
        }
}