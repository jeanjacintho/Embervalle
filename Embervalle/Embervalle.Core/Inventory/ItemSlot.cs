using System;

namespace Embervalle.Core.Inventory
{
    public sealed class ItemSlot
    {
        public ItemInstance? Item { get; set; }

        public bool IsLocked { get; set; }

        public bool IsEmpty => Item == null;

        public bool IsFull =>
            Item != null && Item.Quantity >= Item.GetData().MaxStackSize;

        
        public int TryAdd(ItemInstance incoming)
        {
            if (IsLocked)
            {
                return incoming.Quantity;
            }

            if (IsEmpty)
            {
                Item = incoming;
                return 0;
            }

            if (Item!.ItemId != incoming.ItemId)
            {
                return incoming.Quantity;
            }

            if (!Item.IsStackable)
            {
                return incoming.Quantity;
            }

            int maxStack = Item.GetData().MaxStackSize;
            int space = maxStack - Item.Quantity;
            int toAdd = Math.Min(space, incoming.Quantity);
            int leftover = incoming.Quantity - toAdd;

            Item.Quantity += toAdd;
            incoming.Quantity = leftover;
            return leftover;
        }

        public ItemInstance? Remove(int qty)
        {
            if (IsEmpty || qty <= 0)
            {
                return null;
            }

            qty = Math.Min(qty, Item!.Quantity);
            ItemInstance removed = Item.CloneWithQuantity(qty);
            Item.Quantity -= qty;
            if (Item.Quantity <= 0)
            {
                Item = null;
            }

            return removed;
        }
    }
}
