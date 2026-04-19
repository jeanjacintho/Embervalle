namespace Embervalle.Core.Inventory
{
    /// <summary>Definição imutável de um tipo de item (tabela de design).</summary>
    public sealed class ItemData
    {
        public required string ItemId { get; init; }

        public required string Name { get; init; }

        public ItemCategory Category { get; init; }

        public ItemRarity Rarity { get; init; }

        public int MaxStackSize { get; init; }

        public float Weight { get; init; }

        public int BaseValue { get; init; }

        /// <summary>Legado / referencia para ferramentas; icones usam <see cref="IconAtlasFrameIndex"/>.</summary>
        public string SpriteId { get; init; } = "";

        /// <summary>
        /// Indice linear na grelha: <see cref="Assets.EmbervalleSheets.WeaponIcons"/> se <see cref="Category"/> for Weapon;
        /// caso contrario <see cref="Assets.EmbervalleSheets.ItemIcons"/>. Celulas 16x16. -1 = sem icone.
        /// </summary>
        public int IconAtlasFrameIndex { get; init; } = -1;

        public string Description { get; init; } = "";

        /// <summary>ID de <see cref="Combat.WeaponData"/> em <see cref="Combat.WeaponDefinitions"/>.</summary>
        public string? LinkedWeaponId { get; init; }

        public WeaponStats? Weapon { get; init; }

        public ArmorStats? Armor { get; init; }

        public ConsumableEffect? Consumable { get; init; }

        public bool IsQuestItem { get; init; }

        public bool IsPerishable { get; init; }

        /// <summary>Dias até estragar; 0 = não perece.</summary>
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
