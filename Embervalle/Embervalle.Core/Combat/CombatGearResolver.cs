using Embervalle.Core.Inventory;

namespace Embervalle.Core.Combat
{
    /// <summary>Resolve o <see cref="WeaponData"/> associado a um item de inventário equipado em um slot.</summary>
    public static class CombatGearResolver
    {
        /// <summary>Resolve a arma ligada ao item do slot, se a categoria for arma e existir <see cref="ItemData.LinkedWeaponId"/>.</summary>
        public static bool TryGetWeapon(ItemSlot slot, out WeaponData? weapon)
        {
            weapon = null;
            if (slot.IsEmpty)
            {
                return false;
            }

            return TryFromInstance(slot.Item, out weapon);
        }

        /// <summary>Procura a definição de arma na base de itens a partir de uma <see cref="ItemInstance"/>.</summary>
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
