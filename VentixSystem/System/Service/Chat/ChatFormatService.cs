using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VentixSystem.System.Constant.Chat;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Chat;

namespace VentixSystem.System.Service.Chat
{
        public class ChatFormatService
    {

        public List<ChatTag> GetPlayerTags(UnturnedPlayer player)
        {
            
            //CHECK PERMS
            
            return new List<ChatTag>()
            {
                new ChatTag()
                {
                    Permission = "tag.admin",
                    Prefix = "{color=blue}Admin{/color}",
                    Suffix = ""
                },
                new ChatTag()
                {
                    Permission = "tag.vip",
                    Prefix = "",
                    Suffix = "{color=yellow}VIP{/color}"
                }
            };
        }
        
        public string Format(UnturnedPlayer player, EChatMode mode, string message)
        {
            string formattedMessage = ChatTagsConstants.DEFAULT_FORMAT;
            string chatMode = $"[{GetChatMode(mode)}]";
            if (mode == EChatMode.GLOBAL)
            {
                chatMode = "";
            }

            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            
            
            formattedMessage = formattedMessage.Replace("{CHAT_MODE}", chatMode);
            formattedMessage = formattedMessage.Replace("{PREFIXES}", $"{ventixPlayer.GetPrefix()} ");
            formattedMessage = formattedMessage.Replace("{PLAYER_NAME}", player.DisplayName);
            formattedMessage = formattedMessage.Replace("{MESSAGE}", SerializeMessage(message));

            return formattedMessage;
        }
        

        
        private string GetChatMode(EChatMode mode)
        {
            switch (mode)
            {
                case EChatMode.LOCAL:
                    return "A";
                case EChatMode.GLOBAL:
                    return "W";
                case EChatMode.GROUP:
                    return "G";
                default:
                    return string.Empty;
            }
        }

        private string SerializeMessage(string message)
        {
            string pattern = @"<.*?>(.*?)</.*?>";

            return Regex.Replace(message, pattern, "$1");
        }
    }
}