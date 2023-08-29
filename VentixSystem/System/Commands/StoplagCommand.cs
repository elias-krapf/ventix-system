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
    public class StoplagCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "stoplag";
        
        public string Help { get; }
        
        public string Syntax => "<stoplag>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            
            if (!ventixPlayer.IsAllowedRank(Rank.STAFF))
            {
                UnturnedChat.Say(unturnedPlayer,
                    $"{VentixSystem.Instance.Configuration.Instance.SystemName} You dont have Permission :(",
                    Color.red);
                return;
            }
            
            if (command.Length < 1)
            {
                UnturnedChat.Say(unturnedPlayer,
                    $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /performance <V|A|Z|I>");
                return;
            }

            if (command.Length == 1)
            {
                switch (command[0].ToLower())
                {
                    case "i":
                    case "item":
                    case "items":
                        ItemManager.askClearAllItems();
                        break;
                    case "v":
                    case "vehicle":
                    case "vehicles":
                        VehicleManager.askVehicleDestroyAll();
                        break;
                    case "z":
                    case "zombie":
                    case "zombies":
                        foreach (var zombie in ZombieManager.regions[unturnedPlayer.Player.movement.nav].zombies)
                        {
                            zombie.isDead = true;
                        }

                        break;
                    default:
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /stoplag <V|A|Z|I>");
                        return;
                }
            }
        }
    }
}