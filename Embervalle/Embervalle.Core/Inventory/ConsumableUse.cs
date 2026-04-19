using System;
using Embervalle.Core.Gameplay;

namespace Embervalle.Core.Inventory
{
    public static class ConsumableUse
    {
        public static bool TryUse(ItemSlot slot, PlayerBody player)
        {
            if (slot.IsEmpty)
            {
                return false;
            }

            ItemData data = slot.Item!.GetData();
            if (data.Category != ItemCategory.Consumable || data.Consumable == null)
            {
                return false;
            }

            int heal = data.Consumable.HealAmount;
            if (heal > 0)
            {
                player.Health = Math.Min(player.MaxHealth, player.Health + heal);
            }

            _ = slot.Remove(1);
            return true;
        }
    }
}
