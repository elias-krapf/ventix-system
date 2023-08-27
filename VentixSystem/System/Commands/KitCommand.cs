using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;


namespace VentixSystem.System.Commands
{
    public class KitCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kit";
        
        public string Help { get; }
        
        public string Syntax => "<name>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /kit <name>");
                return;
            }

            var kit = VentixSystem.Instance.Configuration.Instance.Kits.FirstOrDefault(singleKit => singleKit.Name == command[0]);
            if (kit == null)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Kit not found! Use /kits");
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            foreach (var item in kit.Items)
            {
                player.GiveItem(item, 1);
            }
            
            UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received kit {kit.Name}!");
        }
    }
}