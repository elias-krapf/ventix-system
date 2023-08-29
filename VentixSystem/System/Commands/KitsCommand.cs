using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using VentixSystem.System.Constant.Kits;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Kit;

namespace VentixSystem.System.Commands
{
    public class KitsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "kits";
        
        public string Help { get; }
        
        public string Syntax => "<kits>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);

            List<Kit> kits = new List<Kit>();
            foreach (var kit in KitsConstants.Kits)
            {
                if (ventixPlayer.IsAllowedRank(kit.MinRank))
                {
                    kits.Add(kit);
                }
            }

            string message = "Your available Kits: \n";
            foreach (var kit in kits)
            {
                message += $"{kit.Name}, ";
            }
            
            UnturnedChat.Say(player, message);
        }
    }
}