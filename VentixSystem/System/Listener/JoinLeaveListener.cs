using Rocket.API;
using Rocket.Unturned.Player;
using VentixSystem.System.Entity;

namespace VentixSystem.System.Listener
{
    public class JoinLeaveListener
    {
        public void OnJoin(UnturnedPlayer unturnedPlayer)
        {
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            ventixPlayer.Load();
            
            
        }

        public void OnLeave(UnturnedPlayer unturnedPlayer)
        {
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            ventixPlayer.Save();
        }
    }
}