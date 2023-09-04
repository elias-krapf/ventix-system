using Rocket.Core.Logging;
using Rocket.Core.Plugins;

using Rocket.Unturned;
using Rocket.Unturned.Events;
using SDG.Unturned;
using VentixSystem.System.Commands;
using VentixSystem.System.Database;
using VentixSystem.System.Listener;

namespace VentixSystem
{
    public class VentixSystem : RocketPlugin<VentixSystemConfiguration>
    {
        public static VentixSystem Instance { get; set; }
        
   
        protected override void Load()
        {
            Instance = this;
            Logger.Log($"{Configuration.Instance.SystemName} System wurde geladen...");
            MySQLUtils.CreateDefaultTables();

            //register events
            UnturnedPlayerEvents.OnPlayerChatted += new PlayerChatListener().PlayerChatted;
            UnturnedPlayerEvents.OnPlayerDead += new PlayerDeathListener().onDead;
            UnturnedPlayerEvents.OnPlayerDeath += new PlayerDeathListener().OnDeath;
            UnturnedPlayerEvents.OnPlayerUpdateStamina += new StaminaListener().HandlePlayerStaminaUpdate;
            PlayerInput.onPluginKeyTick += new FlyCommand().KeyDown;
            
            U.Events.OnPlayerConnected += new JoinLeaveListener().OnJoin;
            U.Events.OnPlayerDisconnected += new JoinLeaveListener().OnLeave;
        }

        protected override void Unload()
        {
            Logger.Log($"{Configuration.Instance.SystemName} System wurde beendet..."); 
            foreach (SteamPlayer player in Provider.clients)
                player.player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowStamina);
        }
        
    }
}