using System.Collections.Generic;

namespace Embervalle.Core.Inventory
{
    /// <summary>Registro global de todos os tipos de item carregados no jogo, indexados por ID.</summary>
    public static class ItemDatabase
    {
        private static readonly Dictionary<string, ItemData> Items = new();

        public static void Register(ItemData data) => Items[data.ItemId] = data;

        public static ItemData Get(string itemId) => Items[itemId];

        public static bool TryGet(string itemId, out ItemData? data) => Items.TryGetValue(itemId, out data);

        public static bool Exists(string itemId) => Items.ContainsKey(itemId);

        public static int Count => Items.Count;

        public static void ResetCatalog() => Items.Clear();

        /// <summary>Carrega o catálogo a partir de ficheiro se existir; senão regista itens de demonstração.</summary>
        public static void RegisterCoreItems()
        {
            if (Items.Count > 0)
            {
                return;
            }

            if (ItemCatalogLoader.TryLoadFile(ItemCatalogLoader.DefaultCatalogPath) && Items.Count > 0)
            {
                return;
            }

            Items.Clear();
            RegisterFallbackDemoItems();
        }

        /// <summary>Conjunto mínimo de itens quando o ficheiro de catálogo não está disponível.</summary>
        private static void RegisterFallbackDemoItems()
        {
            Register(new ItemData
            {
                ItemId = "weapon_basic_sword",
                Name = "Espada básica",
                Category = ItemCategory.Weapon,
                Rarity = ItemRarity.Common,
                MaxStackSize = 1,
                Weight = 3f,
                BaseValue = 40,
                SpriteId = "",
                IconAtlasFrameIndex = 0,
                LinkedWeaponId = Combat.WeaponDefinitions.BasicSword.Id,
                Weapon = new WeaponStats { Damage = 25 },
            });

            Register(new ItemData
            {
                ItemId = "weapon_axe",
                Name = "Machado",
                Category = ItemCategory.Weapon,
                Rarity = ItemRarity.Uncommon,
                MaxStackSize = 1,
                Weight = 5f,
                BaseValue = 90,
                SpriteId = "",
                IconAtlasFrameIndex = 1,
                LinkedWeaponId = Combat.WeaponDefinitions.Axe.Id,
                Weapon = new WeaponStats { Damage = 45 },
            });

            Register(new ItemData
            {
                ItemId = "weapon_short_bow",
                Name = "Arco curto",
                Category = ItemCategory.Weapon,
                Rarity = ItemRarity.Common,
                MaxStackSize = 1,
                Weight = 2f,
                BaseValue = 55,
                SpriteId = "",
                IconAtlasFrameIndex = 2,
                LinkedWeaponId = Combat.WeaponDefinitions.ShortBow.Id,
                Weapon = new WeaponStats { Damage = 20 },
            });

            Register(new ItemData
            {
                ItemId = "ore_iron",
                Name = "Minério de ferro",
                Category = ItemCategory.Resource,
                Rarity = ItemRarity.Common,
                MaxStackSize = 99,
                Weight = 0.25f,
                BaseValue = 8,
                SpriteId = "",
                IconAtlasFrameIndex = 0,
            });

            Register(new ItemData
            {
                ItemId = "food_bread",
                Name = "Pao",
                Category = ItemCategory.Consumable,
                Rarity = ItemRarity.Common,
                MaxStackSize = 30,
                Weight = 0.1f,
                BaseValue = 5,
                SpriteId = "",
                IconAtlasFrameIndex = 1,
                Consumable = new ConsumableEffect { HealAmount = 20 },
            });
        }
    }
}
