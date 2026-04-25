namespace Embervalle.Core.Inventory
{
    /// <summary>Dados imutáveis que definem um tipo de item: nome, categoria, raridade e atributos.</summary>
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

    /// <summary>Atributos de combate de uma arma.</summary>
    public sealed class WeaponStats
    {
        public int Damage { get; init; }
    }

    /// <summary>Atributos defensivos de uma armadura, incluindo slot e defesa.</summary>
    public sealed class ArmorStats
    {
        public ArmorSlot Slot { get; init; }

        public int Defense { get; init; }
    }

    /// <summary>Efeito aplicado ao jogador ao usar um item consumível.</summary>
    public sealed class ConsumableEffect
    {
        public int HealAmount { get; init; }
    }
}
