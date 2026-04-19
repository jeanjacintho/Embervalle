using Embervalle.Core.Inventory;

namespace Embervalle.Core.Combat
{
    public static class CombatGearResolver
    {
        public static bool TryGetWeapon(ItemSlot slot, out WeaponData? weapon)
        {
            weapon = null;
            if (slot.IsEmpty)
            {
                return false;
            }

            return TryFromInstance(slot.Item, out weapon);
        }

        private static bool TryFromInstance(ItemInstance? inst, out WeaponData? weapon)
        {
            weapon = null;
            if (inst == null || !ItemDatabase.TryGet(inst.ItemId, out ItemData? data) || data is null)
            {
                return false;
            }

            if (data.Category != ItemCategory.Weapon || string.IsNullOrEmpty(data.LinkedWeaponId))
            {
                return false;
            }

            WeaponData? wd = WeaponDefinitions.TryGet(data.LinkedWeaponId);
            if (wd == null)
            {
                return false;
            }

            weapon = wd;
            return true;
        }
    }
}
