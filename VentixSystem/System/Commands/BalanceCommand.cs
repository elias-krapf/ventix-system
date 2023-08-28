using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using VentixSystem.System.Entity;

namespace VentixSystem.System.Commands
{
    public class BalanceCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "balance";
        
        public string Help { get; }
        
        public string Syntax => "balance";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            
            UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your balance is ${ventixPlayer.Balance}");
        }
    }
}