using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Embervalle.Core.Inventory
{
    /// <summary>
    /// Carrega definicoes de itens de JSON (fonte unica para design).
    /// Caminho padrao: pasta do executavel / Data / item_catalog.json
    /// </summary>
    public static class ItemCatalogLoader
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        public static string DefaultCatalogPath =>
            Path.Combine(AppContext.BaseDirectory, "Data", "item_catalog.json");

        public static bool TryLoadFile(string absolutePath, Action<string>? onError = null)
        {
            try
            {
                if (!File.Exists(absolutePath))
                {
                    onError?.Invoke($"Item catalog not found: {absolutePath}");
                    return false;
                }

                string json = File.ReadAllText(absolutePath);
                return TryLoadJson(json, onError);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
                return false;
            }
        }

        public static bool TryLoadJson(string json, Action<string>? onError = null)
        {
            try
            {
                ItemCatalogRoot? root = JsonSerializer.Deserialize<ItemCatalogRoot>(json, JsonOptions);
                if (root?.Items == null || root.Items.Count == 0)
                {
                    onError?.Invoke("Item catalog JSON has no items.");
                    return false;
                }

                ItemDatabase.ResetCatalog();
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (ItemRecordJson row in root.Items)
                {
                    if (string.IsNullOrWhiteSpace(row.ItemId))
                    {
                        onError?.Invoke("Item with empty itemId skipped.");
                        continue;
                    }

                    if (!seen.Add(row.ItemId))
                    {
                        onError?.Invoke($"Duplicate itemId: {row.ItemId}");
                        continue;
                    }

                    ItemData? data = MapToItemData(row, onError);
                    if (data != null)
                    {
                        ItemDatabase.Register(data);
                    }
                }

                return ItemDatabase.Count > 0;
            }
            catch (JsonException ex)
            {
                onError?.Invoke($"JSON: {ex.Message}");
                return false;
            }
        }

        private static ItemData? MapToItemData(ItemRecordJson row, Action<string>? onError)
        {
            if (!TryParseEnum(row.Category, out ItemCategory category))
            {
                onError?.Invoke($"Unknown category '{row.Category}' for {row.ItemId}");
                return null;
            }

            if (!TryParseEnum(row.Rarity, out ItemRarity rarity))
            {
                onError?.Invoke($"Unknown rarity '{row.Rarity}' for {row.ItemId}");
                return null;
            }

            WeaponStats? weapon = null;
            if (row.Weapon != null)
            {
                weapon = new WeaponStats { Damage = row.Weapon.Damage };
            }

            ArmorStats? armor = null;
            if (row.Armor != null)
            {
                if (!TryParseEnum(row.Armor.Slot, out ArmorSlot slot))
                {
                    onError?.Invoke($"Unknown armor slot '{row.Armor.Slot}' for {row.ItemId}");
                    return null;
                }

                armor = new ArmorStats { Slot = slot, Defense = row.Armor.Defense };
            }

            ConsumableEffect? consumable = null;
            if (row.Consumable != null)
            {
                consumable = new ConsumableEffect { HealAmount = row.Consumable.HealAmount };
            }

            return new ItemData
            {
                ItemId = row.ItemId.Trim(),
                Name = string.IsNullOrEmpty(row.Name) ? row.ItemId : row.Name,
                Category = category,
                Rarity = rarity,
                MaxStackSize = row.MaxStackSize,
                Weight = row.Weight,
                BaseValue = row.BaseValue,
                SpriteId = row.SpriteId ?? "",
                IconAtlasFrameIndex = row.IconFrame ?? -1,
                Description = row.Description ?? "",
                LinkedWeaponId = string.IsNullOrWhiteSpace(row.LinkedWeaponId) ? null : row.LinkedWeaponId.Trim(),
                Weapon = weapon,
                Armor = armor,
                Consumable = consumable,
                IsQuestItem = row.IsQuestItem,
                IsPerishable = row.IsPerishable,
                FreshnessMax = row.FreshnessMax,
            };
        }

        private static bool TryParseEnum<T>(string? raw, out T value)
            where T : struct, Enum
        {
            value = default;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            return Enum.TryParse(raw.Trim(), ignoreCase: true, out value);
        }

        private sealed class ItemCatalogRoot
        {
            [JsonPropertyName("items")]
            public List<ItemRecordJson>? Items { get; set; }
        }

        private sealed class ItemRecordJson
        {
            public string ItemId { get; set; } = "";

            public string Name { get; set; } = "";

            public string Category { get; set; } = "Resource";

            public string Rarity { get; set; } = "Common";

            public int MaxStackSize { get; set; } = 1;

            public float Weight { get; set; }

            public int BaseValue { get; set; }

            public string? SpriteId { get; set; }

            /// <summary>Indice no atlas de icones 16x16 (grelha 8x2 por defeito).</summary>
            public int? IconFrame { get; set; }

            public string? Description { get; set; }

            public string? LinkedWeaponId { get; set; }

            public WeaponJson? Weapon { get; set; }

            public ArmorJson? Armor { get; set; }

            public ConsumableJson? Consumable { get; set; }

            public bool IsQuestItem { get; set; }

            public bool IsPerishable { get; set; }

            public int FreshnessMax { get; set; }
        }

        private sealed class WeaponJson
        {
            public int Damage { get; set; }
        }

        private sealed class ArmorJson
        {
            public string Slot { get; set; } = "Head";

            public int Defense { get; set; }
        }

        private sealed class ConsumableJson
        {
            public int HealAmount { get; set; }
        }
    }
}
