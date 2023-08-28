using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;
using VentixSystem.System.Database;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Entity
{
    public class VentixPlayer
    {

        private UnturnedPlayer Player;

        public static Dictionary<ulong, VentixPlayer> ventixPlayers = new Dictionary<ulong, VentixPlayer>();
        
        public static VentixPlayer FetchPlayer(UnturnedPlayer unturnedPlayer)
        {
            if (ventixPlayers.ContainsKey(unturnedPlayer.SteamProfile.SteamID64))
            {
                return ventixPlayers[unturnedPlayer.SteamProfile.SteamID64];
            }
            else
            {
                ventixPlayers.Add(unturnedPlayer.SteamProfile.SteamID64, new VentixPlayer(unturnedPlayer));
                return FetchPlayer(unturnedPlayer);
            }
        }
        
        public VentixPlayer(UnturnedPlayer player)
        {
            this.Player = player;
        }

        public UnturnedPlayer GetUnturnedPlayer()
        {
            return Player;
        }

        public int Balance { get; set; }
        
        public Rank Rank { get; set; }
        
        public void Load()
        {
            MySQLUtils.LoadOrCreatePlayer(this);
        }

        public void Save()
        {
            MySQLUtils.UpdatePlayer(this);
        }

        public bool IsAllowedRank(Rank rank)
        {
            switch (rank)
            {
                case Rank.DEFAULT:
                    return true;
                    case Rank.VIP:
                        return Rank == Rank.VIP || Rank == Rank.EPIC || Rank == Rank.MEGA || Rank == Rank.ULTRA || Rank == Rank.FRIEND || Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.EPIC:
                    return Rank == Rank.EPIC || Rank == Rank.MEGA || Rank == Rank.ULTRA || Rank == Rank.FRIEND || Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.MEGA:
                    return  Rank == Rank.MEGA || Rank == Rank.ULTRA || Rank == Rank.FRIEND || Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.ULTRA:
                    return  Rank == Rank.ULTRA || Rank == Rank.FRIEND || Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.FRIEND:
                    return  Rank == Rank.FRIEND || Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.STAFF:
                    return   Rank == Rank.STAFF || Rank == Rank.OWNER;
                case Rank.OWNER:
                    return  Rank == Rank.OWNER;
                default:
                    return false;
            }

            return false;
        }
        
        public string GetPrefix()
        {
            switch (Rank)
            {
                case Rank.VIP:
                    return "[<color=#00FFFF>VIP</color>]";
                case Rank.EPIC:
                    return "[<color=#FFFF00>Epic</color>]";
                case Rank.MEGA:
                    return "[<color=#FFA500>Mega</color>]";
                case Rank.ULTRA:
                    return "[<color=#800080>Ultra</color>]";
                case Rank.FRIEND:
                    return "[<color=#008000>Friend</color>]";
                case Rank.STAFF:
                    return "[<color=#FF00FF>Staff</color>]";
                case Rank.OWNER:
                    return "[<color=#FF0000>Owner</color>]";
                default:
                    return "";
            }
        }
        
        public void OnDeath()
        {
            this.GivePlayerMaxskills();
        }
        
        public void GivePlayerMaxskills()
        {
            Dictionary<UnturnedSkill, byte> maxSkillsLevel = new Dictionary<UnturnedSkill, byte>()
            {
                { UnturnedSkill.Agriculture, 7 },
                { UnturnedSkill.Cardio, 5 },
                { UnturnedSkill.Cooking, 3 },
                { UnturnedSkill.Crafting, 3 },
                { UnturnedSkill.Dexerity, 5 },
                { UnturnedSkill.Diving, 5 },
                { UnturnedSkill.Engineer, 3 },
                { UnturnedSkill.Exercise, 5 },
                { UnturnedSkill.Fishing, 5 },
                { UnturnedSkill.Healing, 7 },
                { UnturnedSkill.Immunity, 5 },
                { UnturnedSkill.Mechanic, 5 },
                { UnturnedSkill.Outdoors, 5 },
                { UnturnedSkill.Overkill, 7 },
                { UnturnedSkill.Parkour, 5 },
                { UnturnedSkill.Sharpshooter, 7 },
                { UnturnedSkill.Sneakybeaky, 7 },
                { UnturnedSkill.Strength, 5 },
                { UnturnedSkill.Survival, 5 },
                { UnturnedSkill.Toughness, 5 },
                { UnturnedSkill.Vitality, 5 },
                { UnturnedSkill.Warmblooded, 5 }
            };

            foreach (var skill in maxSkillsLevel)
            {
                Player.SetSkillLevel(skill.Key, skill.Value);      
            }
        }
        
    }
}