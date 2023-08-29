using Rocket.API;
using System.Xml.Serialization;

namespace VentixSystem
{
    
    [XmlRoot("VentixSystemConfiguration")]
    public class VentixSystemConfiguration: IRocketPluginConfiguration
    {
      
        public string SystemName { get; set; }
        
        public int TpaDuration { get; set; }
        public int TpaCooldown { get; set; }
        public int TpaDelay { get; set; }
        
        public string DatabaseHost { get; set; }

        public string DatabasePort { get; set; }

        public string DatabaseName { get; set; }
        
        public string DatabaseUser { get; set; }
        
        public string DatabasePassword { get; set; }
        
        public void LoadDefaults()
        {
            //defaults
            SystemName = "[Ventix]";
            
            //tpa
            TpaDuration = 20;
            TpaCooldown = 30;
            TpaDelay = 3;
            
            //database
            DatabaseHost = "s";
            DatabasePort = "3306";
            DatabaseName = "s";
            DatabaseUser = "s";
            DatabasePassword = "s";
        }
    }
}