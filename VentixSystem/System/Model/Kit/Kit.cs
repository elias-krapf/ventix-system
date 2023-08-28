using System.Xml.Serialization;

namespace VentixSystem.System.Model.Kit
{
    public class Kit
    {
        public string Name { get; set; }
        
        public int Cooldown { get; set; }
        
        public Rank.Rank MinRank { get; set; }

        public Item[] Items { get; set; }
    }
}