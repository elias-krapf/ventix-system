using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Commands
{
    public class DutyCommand : IRocketCommand
    {
         public AllowedCaller AllowedCaller => AllowedCaller.Player;
         
         public static Dictionary<CSteamID, Rank> RankSaves = new Dictionary<CSteamID, Rank>();

         
        public string Name => "duty";
        
        public string Help { get; }
        
        public string Syntax => "<duty>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);

            if (ventixPlayer.IsAllowedRank(Rank.STAFF) || RankSaves.ContainsKey(unturnedPlayer.CSteamID))
            {
                if (RankSaves.ContainsKey(unturnedPlayer.CSteamID))
                {
                    ventixPlayer.Rank = RankSaves[unturnedPlayer.CSteamID];
                    RankSaves.Remove(unturnedPlayer.CSteamID);
                    
                    UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} It's time for your duty", Color.magenta);
                }
                else
                {
                    RankSaves.Add(unturnedPlayer.CSteamID, ventixPlayer.Rank);
                    ventixPlayer.Rank = Rank.ULTRA;
                    unturnedPlayer.Features.GodMode = false;
                    UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Take a break from your duty", Color.magenta);
                }
            }
            else
            {
                UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have Permission :(", Color.red);
            }
            
        }
    }
}