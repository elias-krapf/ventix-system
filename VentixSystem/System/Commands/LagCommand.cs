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
    public class LagCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "lag";

        public string Help { get; }

        public string Syntax => "<lag>";

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
                    $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /lag <V|A|Z|I> <region?>");
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
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all items");
                        break;
                    case "s":
                    case "structure":
                    case "structures":
                       StructureManager.askClearAllStructures();
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all structures");
                        break;
                    case "v":
                    case "vehicle":
                    case "vehicles":
                        VehicleManager.askVehicleDestroyAll();
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all vehicles");
                        break;
                    case "a":
                    case "animal":
                    case "animals":
                        AnimalManager.askClearAllAnimals();
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all vehicles");
                        break;
                    case "z":
                    case "zombie":
                    case "zombies":
                        foreach (var zombie in ZombieManager.regions[unturnedPlayer.Player.movement.nav].zombies)
                        {
                            zombie.killWithFireExplosion();
                        }

                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} You killed all zombies");
                        break;
                    default:
                        UnturnedChat.Say(unturnedPlayer,
                            $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /stoplag <V|A|Z|I>");
                        return;
                }
            }

            if (command.Length == 2)
            {
                if (command[2].Equals("r") || command[2].Equals("region"))
                {
                    switch (command[0].ToLower())
                    {
                        case "i":
                        case "item":
                        case "items":
                            ItemManager.askClearRegionItems(10, 10);
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all items");
                            break;
                        case "s":
                        case "structure":
                        case "structures":
                            StructureManager.askClearRegionStructures(10, 10);
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cleared all structures");
                            break;
                        case "v":
                        case "vehicle":
                        case "vehicles":
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} Function not implemented yet");
                            break;
                        case "a":
                        case "animal":
                        case "animals":
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} Function not implemented yet");
                            break;
                        case "z":
                        case "zombie":
                        case "zombies":
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} Function not implemented yet");
                            break;
                        default:
                            UnturnedChat.Say(unturnedPlayer,
                                $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /lag <V|A|Z|I> <region?>");
                            return;
                    }
                }
                else
                {
                    UnturnedChat.Say(unturnedPlayer,
                        $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /lag <V|A|Z|I> <region?>");
                }
            }
        }
    }
}