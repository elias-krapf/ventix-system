using Rocket.Unturned.Player;

namespace VentixSystem.System.Listener
{
    public class StaminaListener
    {
        
        public void HandlePlayerStaminaUpdate(Rocket.Unturned.Player.UnturnedPlayer player, byte stamina)
        {
            if (stamina <= 20)
                player.Player.life.serverModifyStamina(100);
        }
        
    }
}