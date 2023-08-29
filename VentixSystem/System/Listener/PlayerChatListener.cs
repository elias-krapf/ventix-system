using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using Rocket.Unturned.Chat;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Chat;
using VentixSystem.System.Service.Chat;

namespace VentixSystem.System.Listener
{
    public class PlayerChatListener
    {

        public static ChatFormatService ChatFormatService = new ChatFormatService();
        
        public void PlayerChatted(UnturnedPlayer player, ref UnityEngine.Color color, string message,
            EChatMode chatMode, ref bool cancel)
        {
            if (message.StartsWith("/")) 
            { 
                return; 
            }
            
            
            cancel = true;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            UnityEngine.Color msgColor = color;
            msgColor = UnturnedChat.GetColorFromName("white", color);
                
            string formattedMsg = ChatFormatService.Format(player, chatMode, message);
            
            if (chatMode == EChatMode.LOCAL)
            {
                float areaRange = 16384f;
                List<Player> playersInRange;
                playersInRange = new List<Player>();
                PlayerTool.getPlayersInRadius(player.Position, areaRange, playersInRange);

                foreach (Player playerInRange in playersInRange)
                {
                    SteamPlayer client = playerInRange.channel.owner;
                    ChatManager.serverSendMessage(formattedMsg, msgColor, player.SteamPlayer(), client, chatMode, ventixPlayer.AvatarIcon, true);
                }
            } else if (chatMode == EChatMode.GROUP)
            {
                
            } else
            {
                string avatarUrl = player.SteamProfile.AvatarIcon.ToString();
                ChatManager.serverSendMessage(formattedMsg, msgColor, player.SteamPlayer(), null, chatMode, avatarUrl, true);
            }
        }
        
    }
}