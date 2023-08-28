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
using UnturnedItems = VentixSystem.System.Helper.UnturnedItems;


namespace VentixSystem.System.Commands
{
    public class KitCommand : IRocketCommand
    {
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
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /kit <name>");
                return;
            }

            var kit = KitsConstants.Kits.FirstOrDefault(singleKit => String.Equals(singleKit.Name, command[0], (StringComparison)StringComparison.CurrentCultureIgnoreCase));
            if (kit == null)
            {
                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Kit not found! Use /kits");
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            foreach (var item in kit.Items)
            {
                if (item.SightId != null)
                {
                    
                }
                
                
            }
            Item MyItem = UnturnedItems.AssembleItem(363, 100, 147, 1007, 8, 149, 17);
            player.Inventory.tryAddItem(MyItem, true);
            
            UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received kit {kit.Name}!");
        }
    }
}