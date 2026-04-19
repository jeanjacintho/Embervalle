using System;

namespace Embervalle.Core.Inventory
{
    internal static class ContainerHelpers
    {
        public static bool TryRemoveFromSlots(ItemSlot[] slots, string itemId, int qty)
        {
            if (qty <= 0 || !HasInSlots(slots, itemId, qty))
            {
                return false;
            }

            int remaining = qty;
            for (int i = 0; i < slots.Length && remaining > 0; i++)
            {
                ItemSlot slot = slots[i];
                if (slot.IsEmpty || slot.Item!.ItemId != itemId)
                {
                    continue;
                }

                int take = Math.Min(remaining, slot.Item.Quantity);
                _ = slot.Remove(take);
                remaining -= take;
            }

            return remaining == 0;
        }

        public static bool HasInSlots(ItemSlot[] slots, string itemId, int qty)
        {
            return CountInSlots(slots, itemId) >= qty;
        }

        public static int CountInSlots(ItemSlot[] slots, string itemId)
        {
            int n = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot s = slots[i];
                if (!s.IsEmpty && s.Item!.ItemId == itemId)
                {
                    n += s.Item.Quantity;
                }
            }

            return n;
        }

        /// <summary>Tenta colocar o item em pilhas existentes e depois em slots vazios.</summary>
        public static int TryAddToSlots(
            ItemSlot[] slots,
            ItemInstance item,
            float maxWeight,
            ref float currentWeight)
        {
            if (item.Quantity <= 0)
            {
                return 0;
            }

            // Empilhar
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot slot = slots[i];
                if (slot.IsLocked || slot.IsEmpty || slot.Item!.ItemId != item.ItemId || !slot.Item.IsStackable)
                {
                    continue;
                }

                int maxStack = slot.Item.GetData().MaxStackSize;
                int space = maxStack - slot.Item.Quantity;
                if (space <= 0)
                {
                    continue;
                }

                float w = ItemDatabase.Get(item.ItemId).Weight;
                int toAdd = Math.Min(space, item.Quantity);
                if (currentWeight + toAdd * w > maxWeight)
                {
                    continue;
                }

                int before = item.Quantity;
                int leftover = slot.TryAdd(item);
                int added = before - leftover;
                currentWeight += added * w;
                if (item.Quantity == 0)
                {
                    return 0;
                }
            }

            // Slots vazios
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot slot = slots[i];
                if (slot.IsLocked || !slot.IsEmpty)
                {
                    continue;
                }

                float w = ItemDatabase.Get(item.ItemId).Weight;
                float addWeight = item.Quantity * w;
                if (currentWeight + addWeight > maxWeight)
                {
                    return item.Quantity;
                }

                _ = slot.TryAdd(item);
                currentWeight += addWeight;
                return 0;
            }

            return item.Quantity;
        }
    }
}
