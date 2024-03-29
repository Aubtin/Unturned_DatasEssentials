﻿using System;
using System.Globalization;
using System.Linq;
using Rocket.Unturned.Items;
using SDG.Unturned;

namespace datathegenius.DatasEssentials
{
    class ItemUtil
    {
        private static IOrderedEnumerable<ItemAsset> _cachedAssets;

        public static Optional<ItemAsset> GetItem(ushort id)
        {
            return Optional<ItemAsset>.OfNullable((ItemAsset)Assets.find(EAssetType.ITEM, id));
        }
        
        public static Optional<ItemAsset> GetItem(string name)
        {
            if (name == null)
            {
                return Optional<ItemAsset>.Empty();
            }

            ushort id;

            if (ushort.TryParse(name, out id))
            {
                return GetItem(id);
            }

            if (_cachedAssets == null)
            {
                _cachedAssets = Assets.find(EAssetType.ITEM)
                    .Cast<ItemAsset>()
                    .Where(i => i.Name != null)
                    .OrderBy(i => i.Id);
            }

            var lastAsset = null as ItemAsset;
            var lastPriority = 0;

            foreach (var asset in _cachedAssets)
            {
                var itemPriority = 0;
                var itemName = asset.Name;

                
                if (itemName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    lastAsset = asset;
                    break;
                }

                if (itemName.StartsWith(name, true, CultureInfo.InvariantCulture))
                {
                    itemPriority = 3;
                }
                else if ((itemName.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    itemPriority = 2;
                }
                else if (name.IndexOf(' ') > 0 && name.Split(' ').All(p => itemName.IndexOf(p, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    itemPriority = 1;
                }

                if (itemPriority > lastPriority)
                {
                    lastAsset = asset;
                    lastPriority = itemPriority;
                }
            }

            return Optional<ItemAsset>.OfNullable(lastAsset);
        }

        public static Optional<T> GetItemAs<T>(string name) where T : ItemAsset
        {
            var optItem = GetItem(name);

            return optItem.IsPresent ? Optional<T>.Of(optItem.Value as T) : Optional<T>.Empty();
        }

        public static Optional<T> GetItemAs<T>(ushort id) where T : ItemAsset
        {
            var optItem = GetItem(id);

            return optItem.IsPresent ? Optional<T>.Of(optItem.Value as T) : Optional<T>.Empty();
        }


        /*
            Unturned weapon metadata structure.
            metadata[0] = sight id byte 1
            metadata[1] = sight id byte 2
            metadata[2] = tactical id byte 1
            metadata[3] = tactical id byte 2
            
            metadata[4] = grip id byte 1
            metadata[5] = grip id byte 2
            metadata[6] = barrel id byte 1
            metadata[7] = barrel id byte 2
            metadata[8] = magazine id byte 1
            metadata[9] = magazine id byte 2
            metadata[10] = ammo
            metadata[11] = firemode
            metadata[12] = ??
            metadata[13] = sight durability
            metadata[14] = tactical durability
            metadata[15] = grip durability
            metadata[16] = barrel durability
            metadata[17] = magazine durability
        */
        public static Optional<Attachment> GetWeaponAttachment(byte[] metadata, AttachmentType type)
        {
            if (metadata.Length < 18)
            {
                return Optional<Attachment>.Empty();
            }

            var indexes = GetAttachIndexes(type);

            var attachDurability = metadata[indexes[2]];
            var attachId = BitConverter.ToUInt16(metadata, indexes[0]);

            return Optional<Attachment>.Of(new Attachment(attachId, attachDurability));
        }

        public static Optional<EFiremode> GetWeaponFiremode(byte[] metadata)
        {
            if (metadata.Length < 18)
            {
                return Optional<EFiremode>.Empty();
            }

            return Optional<EFiremode>.OfNullable((EFiremode)metadata[0xB]);
        }

        public static Optional<byte> GetWeaponAmmo(byte[] metadata)
        {
            if (metadata.Length < 18)
            {
                return Optional<byte>.Empty();
            }

            return Optional<byte>.Of(metadata[0xA]);
        }


        public static Optional<Attachment> GetWeaponAttachment(Item weaponItem, AttachmentType type)
        {
            return GetWeaponAttachment(weaponItem.Metadata, type);
        }

        public static Optional<EFiremode> GetWeaponFiremode(Item weaponItem)
        {
            return GetWeaponFiremode(weaponItem.Metadata);
        }

        public static Optional<byte> GetWeaponAmmo(Item weaponItem)
        {
            return GetWeaponAmmo(weaponItem.Metadata);
        }


        public static void SetWeaponAttachment(Item weaponItem, AttachmentType type, Attachment attach)
        {
            if (weaponItem.Metadata.Length < 18)
            {
                return;
            }

            AssembleAttach(weaponItem, GetAttachIndexes(type), attach);
        }

        public static void SetWeaponFiremode(Item weaponItem, EFiremode firemode)
        {
            if (weaponItem.Metadata.Length < 18)
            {
                return;
            }

            weaponItem.Metadata[11] = (byte)firemode;
        }

        public static void SetWeaponAmmo(Item weaponItem, byte ammo)
        {
            if (weaponItem.Metadata.Length < 18)
            {
                return;
            }

            weaponItem.Metadata[10] = ammo;
        }


        private static void AssembleAttach(Item item, int[] idxs, Attachment attach)
        {
            if (attach == null || attach.AttachmentId == 0) return;

            var attachIdBytes = BitConverter.GetBytes(attach.AttachmentId);

            // 2 bytes for id (uint16)
            item.Metadata[idxs[0]] = attachIdBytes[0];
            item.Metadata[idxs[1]] = attachIdBytes[1];

            // 1 byte for durability (uint8)
            item.Metadata[idxs[2]] = attach.Durability;
        }

        /*
            return an array with 3 values
            0 = id byte 1
            1 = id byte 2
            2 = durability
        */
        private static int[] GetAttachIndexes(AttachmentType attachType)
        {
            switch (attachType)
            {
                case AttachmentType.SIGHT: return new[] { 0, 1, 13 };
                case AttachmentType.TACTICAL: return new[] { 2, 3, 14 };
                case AttachmentType.GRIP: return new[] { 4, 5, 15 };
                case AttachmentType.BARREL: return new[] { 6, 7, 16 };
                case AttachmentType.MAGAZINE: return new[] { 8, 9, 17 };
                default:
                    throw new ArgumentOutOfRangeException(nameof(attachType), attachType, null);
            }
        }


        public enum AttachmentType
        {
            SIGHT,
            BARREL,
            GRIP,
            TACTICAL,
            MAGAZINE
        }

    }
}
