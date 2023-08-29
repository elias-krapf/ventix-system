using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Commands
{
    public class GodCommand : IRocketCommand
    {
         public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "god";
        
        public string Help { get; }
        
        public string Syntax => "<god>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {

            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);

            if (ventixPlayer.IsAllowedRank(Rank.STAFF))
            {
                bool mode = unturnedPlayer.Features.GodMode;
                unturnedPlayer.Features.GodMode = !unturnedPlayer.Features.GodMode;

                if (!mode)
                {
                    unturnedPlayer.Player.life.serverModifyFood(100);
                    unturnedPlayer.Player.life.serverModifyHealth(100);
                    unturnedPlayer.Player.life.serverModifyVirus(100);
                    unturnedPlayer.Player.life.serverModifyWater(100);
                    unturnedPlayer.Player.life.serverModifyWarmth(100);
                    unturnedPlayer.Player.life.simulatedModifyOxygen(100);
                }

                UnturnedChat.Say(unturnedPlayer,
                    mode
                        ? $"{VentixSystem.Instance.Configuration.Instance.SystemName} You disabled God-Mode"
                        : $"{VentixSystem.Instance.Configuration.Instance.SystemName} You enabled God-Mode");
            }
            else
            {
                UnturnedChat.Say(unturnedPlayer,
                    $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have Permission :(",
                    Color.red);
            }

        }
    }
}