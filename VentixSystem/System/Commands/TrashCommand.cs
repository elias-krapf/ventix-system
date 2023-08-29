using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace VentixSystem.System.Commands
{
    public class TrashCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "trash";
        
        public string Help { get; }
        
        public string Syntax => "<trash>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;

            if (command.Length != 0)
            {
                UnturnedChat.Say(unturnedPlayer,
                    $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /trash");
                return;
            }

            var items = new Items(7);
            items.resize(15, 30);
            unturnedPlayer.Player.inventory.updateItems(7, items);
            unturnedPlayer.Player.inventory.sendStorage();
        }
    }
}