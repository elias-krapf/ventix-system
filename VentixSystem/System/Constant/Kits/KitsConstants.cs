using VentixSystem.System.Model.Kit;

namespace VentixSystem.System.Constant.Kits
{
    public class KitsConstants
    {
        public static Kit[] Kits = new Kit[]
        {       new Kit()
            {
                Items = new[]
                {
                    new Item
                    {
                        ItemId = 363,
                        SightId = 147,
                        TacticalId = 1007,
                        BarrelId = 149,
                        GripId= 8,
                        MagazineId= 17,
                        ClipSize = 100
                    }
                }
            }
        };
    }
}