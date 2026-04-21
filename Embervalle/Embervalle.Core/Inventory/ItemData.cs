namespace Embervalle.Core.Inventory
{
    
    public sealed class ItemData
    {
        public required string ItemId { get; init; }

        public required string Name { get; init; }

        public ItemCategory Category { get; init; }

        public ItemRarity Rarity { get; init; }

        public int MaxStackSize { get; init; }

        public float Weight { get; init; }

        public int BaseValue { get; init; }

        
        public string SpriteId { get; init; } = "";

        
        public int IconAtlasFrameIndex { get; init; } = -1;

        public string Description { get; init; } = "";

        
        public string? LinkedWeaponId { get; init; }

        public WeaponStats? Weapon { get; init; }

        public ArmorStats? Armor { get; init; }

        public ConsumableEffect? Consumable { get; init; }

        public bool IsQuestItem { get; init; }

        public bool IsPerishable { get; init; }

        
        public int FreshnessMax { get; init; }
    }

    public sealed class WeaponStats
    {
        public int Damage { get; init; }
    }

    public sealed class ArmorStats
    {
        public ArmorSlot Slot { get; init; }

        public int Defense { get; init; }
    }

    public sealed class ConsumableEffect
    {
        public int HealAmount { get; init; }
    }
}
