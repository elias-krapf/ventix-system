using Rocket.API;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using UnityEngine;
using Steamworks;
using VentixSystem.System.Model.Fly;

namespace VentixSystem.System.Commands
{
    public class FlyCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "fly";
        public string Help => "Enables/Disables flying mode";
        public string Syntax => "nothing || <player>/all";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> {  };

        public List<CSteamID> InFly = new List<CSteamID>();
        
        public float DefaultSpeedInFly = 10f;
        public float FlyUpSpeed = 0.3f;
        public readonly float Gravity = 0f;
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var main = VentixSystem.Instance;
            var config = main.Configuration.Instance;

            if (args.Length == 1)
            {
                if (args[0].ToLower() == "all")
                {
                    foreach (SteamPlayer sp in Provider.clients)
                    {
                        UnturnedPlayer target = UnturnedPlayer.FromSteamPlayer(sp);
                        FlyComponent cp = target.GetComponent<FlyComponent>();
                        if (cp.isFlying)
                        {
                            cp.FlySpeed = DefaultSpeedInFly;
                            FlyMode(target, false);
                        }
                        else if (!cp.isFlying)
                        {
                            cp.FlySpeed = DefaultSpeedInFly;
                            FlyMode(target, true);
                        }
                    }

                    UnturnedChat.Say(caller, main.Translate("Fly_changed_all"));
                }
                else if (args[0].ToLower() != "all")
                {
                    UnturnedPlayer target = UnturnedPlayer.FromName(args[0]);
                    if (target == null)
                    {
                        UnturnedChat.Say(caller, main.Translate("Player_Not_Found", args[0].ToString()));
                        return;
                    }
                    else
                    {
                        FlyComponent cp = target.GetComponent<FlyComponent>();
                        if (cp.isFlying)
                        {
                            cp.FlySpeed = DefaultSpeedInFly;
                            FlyMode(target, false);
                        }
                        else if (!cp.isFlying)
                        {
                            cp.FlySpeed = DefaultSpeedInFly;
                           FlyMode(target, true);
                        }
                    }
                }
            }
            else if (args.Length == 0)
            {
                FlyComponent cp = player.GetComponent<FlyComponent>();

                if (cp.isFlying)
                {
                    cp.FlySpeed = DefaultSpeedInFly;
                    FlyMode(player, false);
                }
                else if (!cp.isFlying)
                {
                    cp.FlySpeed = DefaultSpeedInFly;
                    FlyMode(player, true);
                }
            }
            else
            {
                UnturnedChat.Say(caller, main.Translate("Fly_Usage" + Syntax));
                return;
            }
        }
        
         public void KeyDown(Player player, uint simulation, byte key, bool state)
        {
            UnturnedPlayer p = UnturnedPlayer.FromPlayer(player);
            FlyComponent cp = p.GetComponent<FlyComponent>();

            if (cp.isFlying)
            {
               
                
                
                if (key == 0 && state)
                {
                    if (cp.FlySpeed == 0)
                    {
                        cp.FlySpeed = DefaultSpeedInFly;
                    }
                    cp.FlySpeed = cp.FlySpeed + 1;
                    p.Player.movement.sendPluginGravityMultiplier(Gravity);
                    p.Player.movement.sendPluginSpeedMultiplier(cp.FlySpeed);
                    
                }
                if (key == 1 && state)
                {
                    if (cp.FlySpeed - 1 > 1)
                    {
                        if (cp.FlySpeed == 0)
                        {
                            cp.FlySpeed = DefaultSpeedInFly;
                        }
                        cp.FlySpeed = cp.FlySpeed - 1;
                        p.Player.movement.sendPluginGravityMultiplier(Gravity);
                        p.Player.movement.sendPluginSpeedMultiplier(cp.FlySpeed);
                    }
                }
            }
        }
         
          public void FlyMode(UnturnedPlayer player, bool enabled)
        {
            FlyComponent cp = player.GetComponent<FlyComponent>();

            float flyspeed = DefaultSpeedInFly;
            if (cp.FlySpeed != 0)
            {
                flyspeed = cp.FlySpeed;
            }

            if (cp.isFlying && !enabled)
            {
                cp.isFlying = false;
                InFly.Remove(player.CSteamID);
                player.Player.movement.sendPluginGravityMultiplier(1f);
                player.Player.movement.sendPluginSpeedMultiplier(1f);
                
                UnturnedChat.Say(player, "fly stop");
            }
            else if (!cp.isFlying && enabled)
            {
                cp.isFlying = true;
                InFly.Add(player.CSteamID);
                player.Player.movement.sendPluginGravityMultiplier(Gravity);
                player.Player.movement.sendPluginSpeedMultiplier(flyspeed);
                
                UnturnedChat.Say(player, "fly start");
            }
        }
          
    }
}