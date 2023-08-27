using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace VentixSystem
{
    public class VentixSystem : RocketPlugin<VentixSystemConfiguration>
    {
        public static VentixSystem Instance { get; set; }
        
        protected override void Load()
        {
            Instance = this;
            Logger.Log($"{Configuration.Instance.SystemName} System wurde geladen...");       
        }

        protected override void Unload()
        {
            Logger.Log($"{Configuration.Instance.SystemName} System wurde beendet...");       
        }
    }
}