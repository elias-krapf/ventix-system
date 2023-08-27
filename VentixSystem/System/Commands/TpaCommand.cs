using System.Collections.Generic;
using Rocket.API;
using NotImplementedException = System.NotImplementedException;

namespace VentixSystem.System.Commands
{
    public class TpaCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "tpa";
        
        public string Help { get; }
        
        public string Syntax => "<name>";
        
        public List<string> Aliases => new List<string>();
        
        public List<string> Permissions => new List<string>();
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            throw new NotImplementedException();
        }
    }
}