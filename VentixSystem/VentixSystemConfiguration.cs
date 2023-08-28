using Rocket.API;
using System.Xml.Serialization;

namespace VentixSystem
{
    
    [XmlRoot("VentixSystemConfiguration")]
    public class VentixSystemConfiguration: IRocketPluginConfiguration
    {
      
        public string SystemName { get; set; }
        
        public string DatabaseHost { get; set; }

        public string DatabasePort { get; set; }

        public string DatabaseName { get; set; }
        
        public string DatabaseUser { get; set; }
        
        public string DatabasePassword { get; set; }
        
        public void LoadDefaults()
        {
            //defaults
            SystemName = "[Ventix]";
            
            //database
            DatabaseHost = "localhost";
            DatabasePort = "3306";
            DatabaseName = "unturned";
            DatabaseUser = "unturned";
            DatabasePassword = "mypassword";
        }
    }
}