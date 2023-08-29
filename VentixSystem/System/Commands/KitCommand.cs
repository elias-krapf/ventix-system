using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Items;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using VentixSystem.System.Constant.Kits;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;
using UnturnedItems = VentixSystem.System.Helper.UnturnedItems;


namespace VentixSystem.System.Commands
{
    public class KitCommand : IRocketCommand
    {
 
        public static Dictionary<ulong, DateTime> InvididualCooldown = new Dictionary<ulong, DateTime>();
        
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
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /kit <kit>");
                return;
            }

            if (command.Length > 2)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /kit <kit> <player>");
                return;
            }
            
            var kit = KitsConstants.Kits.FirstOrDefault(singleKit => String.Equals(singleKit.Name, command[0], (StringComparison)StringComparison.CurrentCultureIgnoreCase));
            if (kit == null)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Kit not found! Use /kits");
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            if (!ventixPlayer.IsAllowedRank(kit.MinRank))
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You need to be {kit.MinRank} to use this kit!", Color.red);
                
                if (kit.MinRank != Rank.FRIEND && kit.MinRank != Rank.STAFF && kit.MinRank != Rank.OWNER)
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Get {kit.MinRank} with /donate!", Color.red);
                }
                return;
            }
            
            if (InvididualCooldown.ContainsKey(player.SteamProfile.SteamID64))
            {
                DateTime timeStamp = InvididualCooldown[player.SteamProfile.SteamID64];
                TimeSpan cooldownDuration = TimeSpan.FromSeconds(kit.Cooldown);
                DateTime cooldownEndTime = timeStamp.Add(cooldownDuration);

                if (cooldownEndTime > DateTime.Now && !ventixPlayer.IsAllowedRank(Rank.OWNER))
                {
                    TimeSpan remainingTime = cooldownEndTime - DateTime.Now;
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You have to wait {remainingTime.TotalSeconds:F0} seconds.", Color.red);
                    return;
                }

                InvididualCooldown.Remove(player.SteamProfile.SteamID64);
            }

            List<UnturnedPlayer> targets = new List<UnturnedPlayer>(){player};
            
            if (command.Length == 2)
            {
                string name = command[1];
                if (!name.Equals("*"))
                {
                    UnturnedPlayer current = UnturnedPlayer.FromName(command[1]);
                    if (current == null)
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                        return;
                    }
                    targets = new List<UnturnedPlayer>() { current };
                }
                else
                {

                    if (ventixPlayer.IsAllowedRank(Rank.OWNER))
                    {
                        targets.Clear();
                        
                        foreach (var steamPlayer in Provider.clients)
                        {
                            targets.Add(UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID));
                        }   
                    }
                    else
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You are not allowed to use '*'.", Color.red);
                        return;
                    }
                }
            }
            
            foreach (var target in targets)
            {
                foreach (var item in kit.Items)
                {

                    bool[] requirementsForWeapon = new bool[]
                    {
                        item.SightId != null,
                        item.TacticalId != null,
                        item.GripId != null,
                        item.BarrelId != null,
                        item.MagazineId != null,
                        item.ClipSize != null,
                    };

                    bool isWeapon = true;
                    foreach (var requirement in requirementsForWeapon)
                    {
                        if (!requirement)
                        {
                            isWeapon = false;
                            break;
                        }
                    }

                    if (isWeapon)
                    {
                        Item MyItem = UnturnedItems.AssembleItem(item.ItemId, (byte)item.ClipSize, (int)item.SightId, (int)item.TacticalId, (int)item.GripId, (int)item.BarrelId, (int)item.MagazineId);
                        target.Inventory.tryAddItem(MyItem, true);
                        continue;
                    }

                    target.GiveItem(item.ItemId, (byte)item.Amount);

                }   
            }

            if (targets.Count == 1)
            {

                var target = targets[0];
                if (player.SteamProfile.SteamID64 == target.SteamProfile.SteamID64)
                {
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received kit {kit.Name}!");   
                }
                else
                {
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received kit {kit.Name} from {player.DisplayName}!");
                    UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} {target.DisplayName} received kit {kit.Name}!");
                }
            }
            else
            {
                foreach (var target in targets)
                {
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received kit {kit.Name} from {player.DisplayName}!");
                }
            }
            
            InvididualCooldown.Add(player.SteamProfile.SteamID64, DateTime.Now);
        }
    }
}