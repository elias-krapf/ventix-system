using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VentixSystem.System.Database;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;
using VentixSystem.System.Model.Vault;

namespace VentixSystem.System.Commands
{
    internal class VaultCommand : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>();
            }
        }

        public string Help
        {
            get
            {
                return "Store items in your vault";
            }
        }

        public string Name
        {
            get
            {
                return "vault";
            }
        }

        public string Syntax
        {
            get
            {
                return "";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "itemvault.vault" };
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            VentixPlayer totalVentixPlayer = VentixPlayer.FetchPlayer(player);

            UnturnedPlayer target = player;
            
            if (command.Length == 1 && ventixPlayer.IsAllowedRank(Rank.STAFF))
            {
                UnturnedPlayer currentTarget = UnturnedPlayer.FromName(command[0]);
                if (target == null)
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                    return;
                }

                target = currentTarget;
                ventixPlayer = VentixPlayer.FetchPlayer(target);
            }
            
            ItemVaultPlayerComponent component = target.Player.transform.GetComponent<ItemVaultPlayerComponent>();
            Items inventory = new Items(7);
            double totalSeconds = (DateTime.Now - component.LastUsed).TotalSeconds;
            if (totalSeconds < 2)
            {
                UnturnedChat.Say(caller, string.Concat(VentixSystem.Instance.Configuration.Instance.SystemName, " Please wait another ", 2 - (int)totalSeconds, " seconds before you use again"), Color.red);
                return;
            }
            component.LastUsed = DateTime.Now;
            
            byte height = 4;
            byte width = 10;

            switch (ventixPlayer.Rank)
            {
                case Rank.VIP:
                    width = 10;
                    height = 8;
                    break;
                case Rank.EPIC:
                    width = 12;
                    height = 16;
                    break;
                case Rank.MEGA:
                    width = 12;
                    height = 24;
                    break;
                case Rank.ULTRA:
                    width = 12;
                    height = 32;
                    break;
                case Rank.FRIEND:
                    width = 12;
                    height = 40;
                    break;
                case Rank.STAFF:
                case Rank.OWNER:
                    width = 12;
                    height = 48;
                    break;
            }
            
            
            inventory.resize(width, height);

            List<ItemJar> items = MySQLUtils.RetrieveItems(target.CSteamID);
            foreach (ItemJar item in items)
            {
                ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
                bool failed = false;
                try
                {

                    if ((itemAsset.size_y+item.y-1) > height)
                    {
                        byte x;
                        byte y;
                        byte rot;
                        failed = !inventory.tryFindSpace(item.x,item.y,out x,out y,out rot);
                        if(!failed)
                            inventory.addItem(x, y, rot, item.item);
                    }
                    else
                    {
                        inventory.addItem(item.x, item.y, item.rot, item.item);
                    }

                }
                catch (Exception)
                {
                    failed = true;
                }

                if (failed)
                {
                    MySQLUtils.DeleteItem(target.CSteamID, item);
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Items were found in your Vault in an area that is no longer yours. The items have been deleted", Color.red);
                }
            }

            if (target.CSteamID == player.CSteamID || totalVentixPlayer.IsAllowedRank(Rank.OWNER))
            {
                inventory.onItemAdded = (byte page, byte index, ItemJar jar) => {
                    MySQLUtils.AddItem(target.CSteamID, jar);
                };
            
                inventory.onItemRemoved = (byte page, byte index, ItemJar jar) => {
                    MySQLUtils.DeleteItem(target.CSteamID, jar);
                };


                inventory.onItemUpdated = (byte page, byte index, ItemJar jar) =>
                {
                    MySQLUtils.UpdateItem(target.CSteamID, jar);
                };   
            }

            player.Player.inventory.updateItems(7, inventory);
            player.Player.inventory.sendStorage();
        }

        
    }
}