using System;

namespace Embervalle.Core.Inventory
{
    /// <summary>
    /// Cria pilhas a partir do catalogo (<see cref="ItemDatabase"/> / item_catalog.json).
    /// Chamar apenas depois de <see cref="ItemDatabase.RegisterCoreItems"/>.
    /// </summary>
    public static class ItemInstanceFactory
    {
        /// <param name="quantity">Quantidade inicial (limitada a <see cref="ItemData.MaxStackSize"/>).</param>
        /// <exception cref="InvalidOperationException">Se <paramref name="itemId"/> nao existir no catalogo.</exception>
        public static ItemInstance Create(string itemId, int quantity = 1)
        {
            if (!ItemDatabase.TryGet(itemId, out ItemData? data) || data is null)
            {
                throw new InvalidOperationException(
                    $"Item '{itemId}' is not registered. Load the catalog before creating instances.");
            }

            int q = Math.Clamp(quantity, 1, data.MaxStackSize);
            int freshness = data.IsPerishable && data.FreshnessMax > 0 ? data.FreshnessMax : 0;

            return new ItemInstance
            {
                ItemId = itemId,
                Quantity = q,
                Freshness = freshness,
                Quality = ItemQuality.Normal,
            };
        }

        /// <summary>Tenta criar; falha silenciosamente se o id for desconhecido (mods / dados opcionais).</summary>
        public static bool TryCreate(string itemId, int quantity, out ItemInstance? instance)
        {
            instance = null;
            if (!ItemDatabase.TryGet(itemId, out ItemData? data) || data is null)
            {
                return false;
            }

            instance = Create(itemId, quantity);
            return true;
        }
    }
}
