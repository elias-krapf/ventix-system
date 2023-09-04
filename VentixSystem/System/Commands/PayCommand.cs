using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;
using NotImplementedException = System.NotImplementedException;

namespace VentixSystem.System.Commands
{
    public class PayCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "pay";
        public string Help => "pay player";
        public string Syntax => Name;
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            
            UnturnedPlayer unturnedPlayer = caller as UnturnedPlayer;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);

            if (command.Length != 2)
            {
                UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /pay <player> <amount>");
                return;
            }

            var amount = command[1];
            int value;
            bool isNumeric = int.TryParse(amount, out value);

            if (!isNumeric)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Amount must be numeric!", Color.red);
                return;
            }


            if (value < 1)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Amount must be positive number!", Color.red);
                return;
            }
            
            string targetName = command[0];
            if (!targetName.Equals("*"))
            {
                UnturnedPlayer target =  UnturnedPlayer.FromName(targetName);
                VentixPlayer targetVentixPlayer = VentixPlayer.FetchPlayer(target);
                if (target == null)
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                    return;
                }

                if (target.CSteamID == unturnedPlayer.CSteamID)
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cannot pay yourself", Color.red);
                    return;
                }

                if (targetVentixPlayer.Balance < value)
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have enough balance!", Color.red);
                    return;
                }
                
                ventixPlayer.Balance -= value;
                targetVentixPlayer.Balance += value;
                
                UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You payed {value}$ to {target.DisplayName}", Color.red);
                UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received {value}$ from {unturnedPlayer.DisplayName}", Color.red);
            }
            else
            {
                if (!ventixPlayer.IsAllowedRank(Rank.OWNER))
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You are not allowed to use '*'.", Color.red);
                    return;
                }
                
                
                
                foreach (var steamPlayer in Provider.clients)
                {
                    UnturnedPlayer current = UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID);
                    VentixPlayer currentVentixPlayer = VentixPlayer.FetchPlayer(current); 

                    currentVentixPlayer.Balance += value;
                    UnturnedChat.Say(current, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received {value}$ from {unturnedPlayer.DisplayName}!");
                }   
                        
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You gave everyone {value}$");
                return;
            }
            
            
            throw new NotImplementedException();
        }
    }
}