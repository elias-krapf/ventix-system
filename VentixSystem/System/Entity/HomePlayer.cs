using System;
using System.Collections;
using System.Collections.Generic;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using VentixSystem.System.Commands;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Entity
{
    public class HomePlayer : UnturnedPlayerComponent
    {
        public bool canGoHome;
        private Vector3 _bedPos;
        private byte _bedRot;
        private UnturnedPlayer _player;

        public static readonly Dictionary<UnturnedPlayer, HomePlayer> CurrentHomePlayers = new Dictionary<UnturnedPlayer, HomePlayer>();
        
        public void GoHome(Vector3 bedPos, byte bedRot, UnturnedPlayer player)
        {
           
            _player = player;
            _bedPos = Vector3.up + bedPos + new Vector3(0f, 0f, 0f);
            _bedRot = bedRot;

            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(player);
            long delay = ventixPlayer.IsAllowedRank(Rank.STAFF) ? 0 : 3;

            if (delay > 0)
            {
                UnturnedChat.Say(player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You will be teleported to your bed in {delay} seconds");
            }
            
            TaskDispatcher.QueueOnMainThread(() =>
            {
                canGoHome = true;
                StartCoroutine(DoGoHome());
            }, delay);
        }
        private IEnumerator DoGoHome()
        {
            if (_player.Dead)
            {
                UnturnedChat.Say(_player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You could not be teleported because you died", Color.red);
                canGoHome = false;
                CurrentHomePlayers.Remove(_player);
                yield break;
            }

            if (!canGoHome)
            {
                CurrentHomePlayers.Remove(_player);
                yield break;
            }
            
            UnturnedChat.Say(_player, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You have been teleported to your bed");
            _player.Teleport(_bedPos, _bedRot);
            HomeCommand.InvididualCooldown.Add(_player.SteamProfile.SteamID64, DateTime.Now);
            canGoHome = false;
            CurrentHomePlayers.Remove(_player);
        }
        
    }
}