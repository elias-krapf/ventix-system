using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using UnityEngine;
using VentixSystem.System.Entity;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Commands
{
    public class HomeCommand : IRocketCommand
    {
        
        public static Dictionary<ulong, DateTime> InvididualCooldown = new Dictionary<ulong, DateTime>();
        
        public string Name => "home";

        public string Help => "Teleports you to your bed if you have one.";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public void Execute(IRocketPlayer caller, string[] bed)
        {
            UnturnedPlayer playerId = (UnturnedPlayer)caller;
            HomePlayer homePlayer = playerId.GetComponent<HomePlayer>();
            object[] cont = CheckConfig(playerId);
            if (!(bool)cont[0]) return;
            HomePlayer.CurrentHomePlayers.Add(playerId, homePlayer);
            homePlayer.GoHome((Vector3)cont[1], (byte)cont[2], playerId);
        }

        public object[] CheckConfig(UnturnedPlayer player)
        {
            object[] returnv = { false, null, null };
         
            if (!BarricadeManager.tryGetBed(player.CSteamID, out var bedPos, out var bedRot))
            {
                UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Unfortunately no bed was found", Color.red);
                return returnv;
            }
            
            if (player.Stance == EPlayerStance.DRIVING || player.Stance == EPlayerStance.SITTING)
            {
                UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cannot teleport to your bed while in a vehicle", Color.red);
                return returnv;
            }
            
            if (InvididualCooldown.ContainsKey(player.SteamProfile.SteamID64))
            {
                DateTime timeStamp = InvididualCooldown[player.SteamProfile.SteamID64];
                TimeSpan cooldownDuration = TimeSpan.FromSeconds(30);
                DateTime cooldownEndTime = timeStamp.Add(cooldownDuration);

                VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
                if (cooldownEndTime > DateTime.Now && !ventixPlayer.IsAllowedRank(Rank.OWNER))
                {
                    TimeSpan remainingTime = cooldownEndTime - DateTime.Now;
                    UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You have to wait {remainingTime.TotalSeconds:F0} seconds.", Color.red);
                    return returnv;
                }

                InvididualCooldown.Remove(player.SteamProfile.SteamID64);
            }
           
            object[] returnv2 = { true, bedPos, bedRot };
            return returnv2;
        }
        
    }
}