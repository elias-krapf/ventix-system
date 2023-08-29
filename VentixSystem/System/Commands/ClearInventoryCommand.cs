using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Commands
{
    public class ClearInventoryCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "clearinventory";
        public string Help => "clears inventory";
        public string Syntax => Name;
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = caller as UnturnedPlayer;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            
            if (command.Length > 0)
            {
                if (!ventixPlayer.IsAllowedRank(Rank.STAFF))
                {
                    UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have Permission :(", Color.red);
                    return;
                }

                string playerName = command[0];
                if (!playerName.Equals("*"))
                {
                    UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
                    if (target == null)
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                        return;
                    }
                
                    Clear(target.Player.inventory);
                    Clear(target.Player.inventory);
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared {target.DisplayName}'s inventory!");
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared by {unturnedPlayer.DisplayName}!");
                    return;   
                }
                else
                {
                    if (ventixPlayer.IsAllowedRank(Rank.OWNER))
                    {
                        foreach (var steamPlayer in Provider.clients)
                        {
                            UnturnedPlayer current = UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID);
                            Clear(current.Player.inventory);
                            UnturnedChat.Say(current, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared by {unturnedPlayer.DisplayName}!");
                        }   
                        
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared everyone's inventory!");
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You are not allowed to use '*'.", Color.red);
                        return;
                    }
                }
            }

            Clear(unturnedPlayer.Player.inventory);
            UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared");
        }

        public void Clear(PlayerInventory playerInv)
        {
            var player = playerInv.player;
            var clothing = player.clothing;
            HideWeaponModels(player);
            ClearItems(playerInv);
            ClearClothes(clothing);
            ClearItems(playerInv);
        }

        private void ClearItems(PlayerInventory playerInv)
        {
            for (byte page = 0; page < 6; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = playerInv.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    playerInv.removeItem(page, 0);
                }
            }
        }

        private void ClearClothes(PlayerClothing cloth)
        {
            cloth.askWearBackpack(0, 0, new byte[0], true);
            cloth.askWearGlasses(0, 0, new byte[0], true);
            cloth.askWearHat(0, 0, new byte[0], true);
            cloth.askWearPants(0, 0, new byte[0], true);
            cloth.askWearMask(0, 0, new byte[0], true);
            cloth.askWearShirt(0, 0, new byte[0], true);
            cloth.askWearVest(0, 0, new byte[0], true);

            for (byte i = 0; i < cloth.player.inventory.getItemCount(2); i++)
            {
                cloth.player.inventory.removeItem(2, 0);
            }
        }

        private void HideWeaponModels(Player player)
        {

            player.channel.send("tellSlot", (ESteamCall)1, (ESteamPacket)15,
                new object[] {
                    0,
                    0,
                    new byte[0] 
                });

            player.channel.send("tellSlot", (ESteamCall)1, (ESteamPacket)15,
            new object[] {
                    1,
                    0,
                    new byte[0]
            });
        }
    }
}