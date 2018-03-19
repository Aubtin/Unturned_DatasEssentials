using System.Globalization;
using System.Linq;
using SDG.Unturned;
using System;

namespace datathegenius.DatasEssentials
{
    class VehicleUtil
    {
        private static IOrderedEnumerable<VehicleAsset> _cachedAssets;

        public static Optional<VehicleAsset> GetVehicle(ushort id)
        {
            return Optional<VehicleAsset>.OfNullable((VehicleAsset)Assets.find(EAssetType.VEHICLE, id));
        }

        public static Optional<VehicleAsset> GetVehicle(string name)
        {
            if (name == null)
            {
                return Optional<VehicleAsset>.Empty();
            }

            ushort id;

            if (ushort.TryParse(name, out id))
            {
                return GetVehicle(id);
            }

            if (_cachedAssets == null)
            {
                _cachedAssets = Assets.find(EAssetType.VEHICLE)
                    .Cast<VehicleAsset>()
                    .Where(i => i.Name != null)
                    .OrderBy(i => i.Id);
            }

            var lastAsset = null as VehicleAsset;
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

            return Optional<VehicleAsset>.OfNullable(lastAsset);
        }
    }
}
