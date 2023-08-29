using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using VentixSystem.System.Commands;
using VentixSystem.System.Entity;

namespace VentixSystem.System.Model.Tpa
{
    public class TPARequest
    {

        public TPARequest(CSteamID sender, CSteamID target)
        {
            Sender = sender;
            Target = target;

            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (TpaCommand.TPRequests.Contains(this))
                {
                    TpaCommand.TPRequests.Remove(this);
                    UnturnedChat.Say(TargetPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} The teleportation from {SenderPlayer.DisplayName} was aborted because you didn't accept in time");
                    UnturnedChat.Say(SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} The teleportation was aborted because {TargetPlayer.DisplayName} didn't accept in time");
                }
            }, 30);
        }
        
        public CSteamID Sender { get; set; }
        public CSteamID Target { get; set; }
        public bool IsCanceled { get; private set; }

        public UnturnedPlayer SenderPlayer => UnturnedPlayer.FromCSteamID(Sender);
        public UnturnedPlayer TargetPlayer => UnturnedPlayer.FromCSteamID(Target);


        public void Execute(double delay)
        {            
            
            VentixPlayer targetVentixPlayer = VentixPlayer.FetchPlayer(TargetPlayer);
            VentixPlayer senderVentixPlayer = VentixPlayer.FetchPlayer(SenderPlayer);
            bool hasDelay = targetVentixPlayer.IsAllowedRank(Rank.Rank.OWNER) || senderVentixPlayer.IsAllowedRank(Rank.Rank.OWNER);

            if (!hasDelay)
            {
                UnturnedChat.Say(TargetPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You accepted Tpa from {SenderPlayer.DisplayName}! Wait 3 seconds");
                UnturnedChat.Say(SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} {TargetPlayer.DisplayName} accepted the Tpa-Request! Wait 3 seconds");   
            }
            else
            {
                UnturnedChat.Say(TargetPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You accepted Tpa from {SenderPlayer.DisplayName}!");
                UnturnedChat.Say(SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} {TargetPlayer.DisplayName} accepted the Tpa-Request!");
            }
            
            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (IsCanceled)
                {
                    return;
                }
                
                if (!Validate(true))
                {
                    return;
                }
                
                long innerDelay = targetVentixPlayer.IsAllowedRank(Rank.Rank.OWNER) ||
                             senderVentixPlayer.IsAllowedRank(Rank.Rank.OWNER)
                    ? 0
                    : 3;

                TaskDispatcher.QueueOnMainThread(() =>
                {
                    SenderPlayer.Teleport(TargetPlayer);    
                }, innerDelay);
            }, (float)delay);
        }

        public bool Validate(bool isFinal = false)
        {

            if (SenderPlayer.Dead || (TargetPlayer.Dead && isFinal))
            {
                UnturnedChat.Say(SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Teleportation could not be performed because you or {TargetPlayer.DisplayName} died");
                return false;
            }
            
            if (SenderPlayer.IsInVehicle)
            {
                UnturnedChat.Say(SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Teleportation could not be performed because you are in a vehicle");
                return false;
            }

            return true;
        }
        
    }
}