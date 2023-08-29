using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Commands
{
    public class RankCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "rank";
        
        public string Help { get; }
        
        public string Syntax => "rank";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);

            if (ventixPlayer.Rank != Rank.OWNER)
            {
                UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have Permission :(", Color.red);
                return;
            }

            if (command.Length < 2)
            {
                UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /rank <rank> <player>", Color.cyan);
                return;
            }

            UnturnedPlayer targetUnturnedPlayer = UnturnedPlayer.FromName(command[1]);
            if (targetUnturnedPlayer == null)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                return;
            }
            
            VentixPlayer targetVentixPlayer = VentixPlayer.FetchPlayer(targetUnturnedPlayer);
            
            switch (command[0].ToLower())
            {
                case "vip":
                    targetVentixPlayer.Rank = Rank.VIP;
                    break;
                case "epic":
                    targetVentixPlayer.Rank = Rank.EPIC;
                    break;
                case "mega":
                    targetVentixPlayer.Rank = Rank.MEGA;
                    break;
                case "ultra":
                    targetVentixPlayer.Rank = Rank.ULTRA;
                    break;
                case "friend":
                    targetVentixPlayer.Rank = Rank.FRIEND;
                    break;
                case "staff":
                    targetVentixPlayer.Rank = Rank.STAFF;
                    break;
                case "owner":
                    targetVentixPlayer.Rank = Rank.OWNER;
                    break;
                default:
                    targetVentixPlayer.Rank = Rank.DEFAULT;
                    break;
            }
            
            UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You updated {targetUnturnedPlayer.DisplayName} group to {targetVentixPlayer.Rank}");
            UnturnedChat.Say(targetUnturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your group was updated to {targetVentixPlayer.Rank} by {player.DisplayName}");
            
        }
    }
}