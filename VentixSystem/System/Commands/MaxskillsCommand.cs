using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;

namespace VentixSystem.System.Commands
{
    public class MaxskillsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public string Name => "maxskills";
        
        public string Help { get; }
        
        public string Syntax => "<maxskill>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

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

            foreach (var x in maxSkillsLevel)
            {
                player.SetSkillLevel(x.Key, x.Value);   
            }

            UnturnedChat.Say(caller, $"{VentixSystem.Instance.Configuration.Instance.SystemName} Your skills have been updated!");
        }
    }
}