using System;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using VentixSystem.System.Entity;

namespace VentixSystem.System.Listener
{
    public class PlayerDeathListener
    {
        public void onDead(UnturnedPlayer unturnedPlayer, Vector3 vector3)
        {
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            ventixPlayer.GivePlayerMaxskills();
        }
        
         public void OnDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
        {
            if (cause.ToString() == "GUN")
            {
                if (limb == ELimb.SKULL)
                {
                    UnturnedChat.Say(VentixSystem.Instance.Configuration.Instance.SystemName + " " + UnturnedPlayer.FromCSteamID(murderer).CharacterName + " [" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP] " + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " [" + Math.Round((double)Vector3.Distance(player.Position, UnturnedPlayer.FromCSteamID(murderer).Position)).ToString() + "M] [Headshot] " + player.DisplayName);
                }
                else if (limb == ELimb.LEFT_ARM || limb == ELimb.LEFT_BACK || limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_FRONT || limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_ARM || limb == ELimb.RIGHT_BACK || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_FRONT || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_LEG || limb == ELimb.SPINE)
                {
                    EffectManager.sendUIEffect(32479, 312, true, UnturnedPlayer.FromCSteamID(murderer).CharacterName + " <color=lime>[" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP]</color> " + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " <color=lime>[" + Math.Round((double)Vector3.Distance(player.Position, UnturnedPlayer.FromCSteamID(murderer).Position)).ToString() + "M]</color> " + player.DisplayName);
                }
                else
                {
                    EffectManager.sendUIEffect(32479, 312, true, UnturnedPlayer.FromCSteamID(murderer).CharacterName + " <color=lime>[" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP]</color> " + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " <color=lime>[" + Math.Round((double)Vector3.Distance(player.Position, UnturnedPlayer.FromCSteamID(murderer).Position)).ToString() + "M]</color> " + player.DisplayName);
                }

            }
            else if (cause.ToString() == "MELEE")
            {
                if (limb == ELimb.SKULL)
                {
                    EffectManager.sendUIEffect(32479, 312, true, UnturnedPlayer.FromCSteamID(murderer).CharacterName + " <color=lime>[" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP]</color> " + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " <color=lime>[HS]</color> " + player.DisplayName);
                }
                else if (limb == ELimb.LEFT_ARM || limb == ELimb.LEFT_BACK || limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_FRONT || limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_ARM || limb == ELimb.RIGHT_BACK || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_FRONT || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_LEG || limb == ELimb.SPINE)
                {
                    EffectManager.sendUIEffect(32479, 312, true, UnturnedPlayer.FromCSteamID(murderer).CharacterName + " <color=lime>[" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP]</color>" + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " " + player.DisplayName);
                }
                else
                {
                    EffectManager.sendUIEffect(32479, 312, true, UnturnedPlayer.FromCSteamID(murderer).CharacterName + " <color=lime>[" + UnturnedPlayer.FromCSteamID(murderer).Health.ToString() + " HP]" + UnturnedPlayer.FromCSteamID(murderer).Player.equipment.asset.itemName.ToString() + " " + player.DisplayName);
                }
            }
        }
    }
}