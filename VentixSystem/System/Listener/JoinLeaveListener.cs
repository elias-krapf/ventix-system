using System;
using SDG.Unturned;
using Rocket.Unturned.Player;
using VentixSystem.System.Entity;

namespace VentixSystem.System.Listener
{
    public class JoinLeaveListener
    {
        public void OnJoin(UnturnedPlayer unturnedPlayer)
        {
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            ventixPlayer.AvatarIcon = unturnedPlayer.SteamProfile.AvatarIcon.ToString();
            ventixPlayer.CurrentPlaytime = DateTime.Now;
            ventixPlayer.Load();
            
            unturnedPlayer.Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowStamina);
        }

        public void OnLeave(UnturnedPlayer unturnedPlayer)
        {
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            ventixPlayer.Save();
        }
    }
}