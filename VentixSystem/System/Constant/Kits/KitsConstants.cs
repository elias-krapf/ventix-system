using VentixSystem.System.Model.Kit;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Constant.Kits
{
    public class KitsConstants
    {
        public static Kit[] Kits =
        {       
            new Kit() {
                Name = "team",
                MinRank = Rank.OWNER,
                Cooldown = 30,
                Items = new[]
                {
                    //maplestrike
                    new Item
                    {
                        ItemId = 363,
                        SightId = 147,
                        TacticalId = 1007,
                        BarrelId = 149,
                        GripId= 8,
                        MagazineId= 17,
                        ClipSize = 100
                    },
                    //honeybedger
                    new Item
                    {
                        ItemId = 116,
                        Amount = 1,
                    },
                    //alicepack
                    new Item
                    {
                        ItemId = 253,
                        Amount = 1
                    },
                    //spec ops top
                    new Item
                    {
                        ItemId = 1171,
                        Amount = 1
                    },
                    //spec ops bottom
                    new Item
                    {
                        ItemId = 1172,
                        Amount = 1
                    },
                    //spec ops vest
                    new Item
                    {
                        ItemId = 1169,
                        Amount = 1
                    },
                    //spec ops helmet
                    new Item
                    {
                        ItemId = 1525,
                        Amount = 1
                    },
                    //mask
                    new Item
                    {
                        ItemId = 1270,
                        Amount = 1
                    },
                    //nozzle
                    new Item
                    {
                        ItemId = 150,
                        Amount = 1
                    },
                    //filter
                    new Item
                    {
                        ItemId = 1271,
                        Amount = 2
                    },
                    //granade
                    new Item
                    {
                        ItemId = 254,
                        Amount = 2
                    },
                    //bloodbag
                    new Item
                    {
                        ItemId = 395,
                        Amount = 5
                    },
                    //military drum
                    new Item
                    {
                        ItemId = 17,
                        Amount = 4,
                    },
                    //mre
                    new Item
                    {
                        ItemId = 81,
                        Amount = 8,
                    }, 
                    //vertical grip
                    new Item
                    {
                        ItemId = 9,
                        Amount = 1,
                    },
                    //adaptive chambering
                    new Item
                    {
                        ItemId = 1007,
                        Amount = 1,
                    },
                    //sight
                    new Item
                    {
                        ItemId = 147,
                        Amount = 1,
                    },
                }
            },
           new Kit(){
                Name = "ultra",
                MinRank = Rank.ULTRA,
                Items = new[]
                {
                    //Zubeknakov
                    new Item
                    {
                        ItemId = 122,
                        SightId = 147,
                        TacticalId = 1007,
                        BarrelId = 1191,
                        GripId= 8,
                        MagazineId= 125,
                        ClipSize = 75
                    },
                    //Desert Falcon
                    new Item
                    {
                        ItemId = 488,
                        SightId = 0,
                        TacticalId = 0,
                        BarrelId = 0,
                        GripId= 0,
                        MagazineId= 489,
                        ClipSize = 7
                    },
                    //Desert Falcon Magazine
                    new Item
                    {
                        ItemId = 489,
						Amount = 4
                    },					
                    //alicepack
                    new Item
                    {
                        ItemId = 253,
                        Amount = 1
                    },
                    //Thief Top
                    new Item
                    {
                        ItemId = 1156,
                        Amount = 1
                    },
                    //Thief Bottom
                    new Item
                    {
                        ItemId = 1157,
                        Amount = 1
                    },
                    //Forest Military Vest
                    new Item
                    {
                        ItemId = 310,
                        Amount = 1
                    },
                    //Forest Military Helmet
                    new Item
                    {
                        ItemId = 309,
                        Amount = 1
                    },
                    //Ranger Muzzle
                    new Item
                    {
                        ItemId = 1190,
                        Amount = 1
                    },
                    //bloodbag
                    new Item
                    {
                        ItemId = 395,
                        Amount = 5
                    },
                    //ranger drum
                    new Item
                    {
                        ItemId = 125,
                        Amount = 2,
                    },
					//Low Caliber Ranger Ammunition Box
                    new Item
                    {
                        ItemId = 119,
                        Amount = 2,
                    },
                    //mre
                    new Item
                    {
                        ItemId = 81,
                        Amount = 4,
                    },
                }
            }
        };
    }
}