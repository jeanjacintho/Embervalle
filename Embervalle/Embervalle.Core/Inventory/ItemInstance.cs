namespace Embervalle.Core.Inventory
{
    
    public sealed class ItemInstance
    {
        public required string ItemId { get; set; }

        public int Quantity { get; set; }

        public int Freshness { get; set; }

        public ItemQuality Quality { get; set; }

        public string? Enchantment { get; set; }

        public int DurabilityMax { get; set; }

        public int DurabilityCurrent { get; set; }

        public ItemData GetData() => ItemDatabase.Get(ItemId);

        public bool IsStackable => GetData().MaxStackSize > 1;

        public bool IsPerishable => GetData().IsPerishable;

        public bool IsMaxDurability => DurabilityMax <= 0 || DurabilityCurrent >= DurabilityMax;

        public ItemInstance CloneWithQuantity(int qty)
        {
            return new ItemInstance
            {
                ItemId = ItemId,
                Quantity = qty,
                Freshness = Freshness,
                Quality = Quality,
                Enchantment = Enchantment,
                DurabilityMax = DurabilityMax,
                DurabilityCurrent = DurabilityCurrent,
            };
        }
    }
}
