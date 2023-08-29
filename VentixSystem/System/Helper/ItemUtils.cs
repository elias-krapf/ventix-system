using System;
using System.Linq;
using SDG.Unturned;

namespace VentixSystem.System.Helper
{
    public class Attachment{
        public ushort AttachmentId = 0;
        public byte Durability = 100;
        public Attachment(ushort attachmentId, byte durability){
            AttachmentId = attachmentId;
            Durability = durability;
        }
    }
    
    public static class UnturnedItems
    {
        public static ItemAsset GetItemAssetByName(string name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            return SDG.Unturned.Assets.find(EAssetType.ITEM).Cast<ItemAsset>().Where(i => i.itemName != null && i.itemName.ToLower().Contains(name.ToLower())).FirstOrDefault();
        }

        public static ItemAsset GetItemAssetById(ushort id)
        {
            Asset asset = SDG.Unturned.Assets.find(EAssetType.ITEM, id);
            if (asset == null) return null;
            return (ItemAsset)asset;
        }

        public static Item AssembleItem(ushort itemId, byte clipsize, int sight, int tactical, int grip, int barrel, int magazine, EFiremode firemode = EFiremode.AUTO, byte amount = 1, byte durability = 100)
        {
            byte[] metadata = new byte[18];

            if (sight != 0)
            {
                byte[] sightBytes = BitConverter.GetBytes(sight);
                metadata[0] = sightBytes[0];
                metadata[1] = sightBytes[1];
                metadata[13] = 100;
            }

            if (tactical != 0)
            {
                byte[] tacticalBytes = BitConverter.GetBytes(tactical);
                metadata[2] = tacticalBytes[0];
                metadata[3] = tacticalBytes[1];
                metadata[14] = 100;
            }

            if (grip != 0)
            {
                byte[] gripBytes = BitConverter.GetBytes(grip);
                metadata[4] = gripBytes[0];
                metadata[5] = gripBytes[1];
                metadata[15] = 100;
            }

            if ( barrel != 0)
            {
                byte[] barrelBytes = BitConverter.GetBytes(barrel);
                metadata[6] = barrelBytes[0];
                metadata[7] = barrelBytes[1];
                metadata[16] = 100;
            }

            if (magazine != 0)
            {
                byte[] magazineBytes = BitConverter.GetBytes(magazine);
                metadata[8] = magazineBytes[0];
                metadata[9] = magazineBytes[1];
                metadata[17] = 100;
            }

            metadata[10] = clipsize;
            metadata[11] = (byte)firemode;
            metadata[12] = 1;

            return AssembleItem(itemId,amount,durability,metadata);
        }

        public static Item AssembleItem(ushort itemId, byte amount = 1, byte durability = 100, byte[] metadata = null)
        {
            return new Item(itemId, amount, durability, (metadata == null ? new byte[0] : metadata));
        }
    }
}