using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using VentixSystem.System.Entity;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;
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
            
            if (command.Length == 0)
            {
                ClearInventory(unturnedPlayer);
                UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared");
                return;
            }

            var name = command[0];
            if (!name.Equals("*"))
            {
                var target = UnturnedPlayer.FromName(command[0]);

                if (target != null)
                {
                    ClearInventory(UnturnedPlayer.FromName(command[0]));
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared {target.DisplayName}'s inventory!");
                    UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared by {unturnedPlayer.DisplayName}!");
                }
                else
                {
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                }   
            }
            else
            {
                if (ventixPlayer.IsAllowedRank(Rank.OWNER))
                {
                    foreach (var steamPlayer in Provider.clients)
                    {
                        UnturnedPlayer current = UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID);
                        ClearInventory(current);
                        UnturnedChat.Say(current, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your inventory was cleared by {unturnedPlayer.DisplayName}!");
                    }   
                        
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared everyone's inventory!");
                    return;
                }
              
                    UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You are not allowed to use '*'.", Color.red);
            }
             
            
        }

        private void ClearInventory(UnturnedPlayer player)
        {
            var playerInv = player.Inventory;
            

            player.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                (byte)0, (byte)0, EMPTY_BYTE_ARRAY);
            player.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                (byte)1, (byte)0, EMPTY_BYTE_ARRAY);


            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = playerInv.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    playerInv.removeItem(page, 0);
                }
            }

            

            player.Player.clothing.askWearBackpack(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearGlasses(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearHat(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearPants(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearMask(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearShirt(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);

            player.Player.clothing.askWearVest(0, 0, EMPTY_BYTE_ARRAY, true);
            removeUnequipped(playerInv);
        }

        public readonly byte[] EMPTY_BYTE_ARRAY = new byte[0];

        public void removeUnequipped(PlayerInventory playerInv)
        {
            for (byte i = 0; i < playerInv.getItemCount(2); i++)
            {
                playerInv.removeItem(2, 0);
            }
        }
    }
}