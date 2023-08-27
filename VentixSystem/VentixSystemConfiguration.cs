using Rocket.API;
using VentixSystem.System.Model;

namespace VentixSystem
{
    public class VentixSystemConfiguration: IRocketPluginConfiguration
    {
      
        public string SystemName { get; set; }
        
        public string DatabaseHost { get; set; }

        public string DatabasePort { get; set; }

        public string DatabaseName { get; set; }
        
        public string DatabaseUser { get; set; }
        
        public string DatabasePassword { get; set; }
        
        public Kit[] Kits { get; set; }
        
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

            Kits = new Kit[]
            {
                new Kit()
                {
                    Name = "Maple",
                    Items = new ushort[]
                    {
                        363,
                        15,
                        15
                    }
                }
            };
        }
    }
}