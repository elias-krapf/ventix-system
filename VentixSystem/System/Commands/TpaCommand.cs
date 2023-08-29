using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using UnityEngine;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;
using VentixSystem.System.Model.Tpa;
using NotImplementedException = System.NotImplementedException;

namespace VentixSystem.System.Commands
{
    public class TpaCommand : IRocketCommand
    {

        public static List<TPARequest> TPRequests = new List<TPARequest>();
        
        public static Dictionary<ulong, DateTime> InvididualCooldown = new Dictionary<ulong, DateTime>();
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "tpa";
        
        public string Help { get; }
        
        public string Syntax => "<name>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>(){};
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;
            VentixPlayer ventixPlayer = VentixPlayer.FetchPlayer(unturnedPlayer);
            
            if (command.Length < 1)
            {
                UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Use /tpa <name | (accept, cancel, deny)>");
                return;
            }

            string cmd = command[0].ToLower();
            switch (cmd)
            {
                case "accept":
                case "a":
                    var acceptRequest = TPRequests.Find(x => x.TargetPlayer.CSteamID == unturnedPlayer.CSteamID);
                    if (acceptRequest == null)
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} There is not Tpa-Request!", Color.red);
                        return;
                    }
                    
                    acceptRequest.Execute(VentixSystem.Instance.Configuration.Instance.TpaDelay);
                    TPRequests.Remove(acceptRequest);
                    break;
                case "cancel":
                case "c":
                    var cancelRequest = TPRequests.FirstOrDefault(x => x.Sender == unturnedPlayer.CSteamID);
                    if (cancelRequest != null)
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You canceled Tpa-Request of {cancelRequest.TargetPlayer.DisplayName}", Color.red);
                        UnturnedChat.Say(cancelRequest.TargetPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} {unturnedPlayer.DisplayName} canceled the Tpa-Request", Color.red);
                        TPRequests.Remove(cancelRequest);
                        InvididualCooldown.Remove(unturnedPlayer.SteamProfile.SteamID64);
                    } else
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} There is not Tpa-Request!", Color.red);
                    }
                    break;
                case "deny":
                case "d":
                    var denyRequest = TPRequests.FirstOrDefault(x => x.Target == unturnedPlayer.CSteamID);
                    if (denyRequest != null)
                    {
                        UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You denied Tpa-Request of {denyRequest.SenderPlayer.DisplayName}", Color.red);
                        UnturnedChat.Say(denyRequest.SenderPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} {unturnedPlayer.DisplayName} denied the Tpa-Request", Color.red);

                        TPRequests.Remove(denyRequest);
                    }
                    else
                    {
                        UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} There is not Tpa-Request!", Color.red);
                    }
                    break;
                default:
                    UnturnedPlayer target = UnturnedPlayer.FromName(cmd);
                    if (target != null)
                    {
                        
                        if (InvididualCooldown.ContainsKey(unturnedPlayer.SteamProfile.SteamID64))
                        {
                            DateTime timeStamp = InvididualCooldown[unturnedPlayer.SteamProfile.SteamID64];
                            TimeSpan cooldownDuration = TimeSpan.FromSeconds(30);
                            DateTime cooldownEndTime = timeStamp.Add(cooldownDuration);

                            if (cooldownEndTime > DateTime.Now && !ventixPlayer.IsAllowedRank(Rank.OWNER))
                            {
                                TimeSpan remainingTime = cooldownEndTime - DateTime.Now;
                                UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You have to wait {remainingTime.TotalSeconds:F0} seconds.", Color.red);
                                return;
                            }

                            InvididualCooldown.Remove(unturnedPlayer.SteamProfile.SteamID64);
                        }
                        
                        
                        if (target.Id == unturnedPlayer.Id)
                        {
                            UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You cannot send yourself a request", Color.red);
                            return;
                        }
                        
                        if (TPRequests.Exists(x => x.Sender == unturnedPlayer.CSteamID && x.Target == target.CSteamID))
                        {
                            UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You already sent Tpa-Request to {target.DisplayName}", Color.yellow);
                            return;
                        }
                        
                        
                        TPARequest request = new TPARequest(unturnedPlayer.CSteamID, target.CSteamID);
                        if (!request.Validate())
                        {
                            return;
                        }
                        
                        TPRequests.Add(request);
                        UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You sent Tpa-Request to {target.DisplayName}", Color.yellow);
                        UnturnedChat.Say(target, $"{VentixSystem.Instance.Configuration.Instance.SystemName} You received Tpa-Request from {unturnedPlayer.DisplayName}", Color.yellow);
                        InvididualCooldown.Add(unturnedPlayer.SteamProfile.SteamID64, DateTime.Now);
                    } else
                    {
                        UnturnedChat.Say(unturnedPlayer, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Player cannot be found!", Color.red);
                    }

                    break;
            }

        }
    }
}