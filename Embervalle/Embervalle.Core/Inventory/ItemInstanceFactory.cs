using System;

namespace Embervalle.Core.Inventory
{
    
    
    public static class ItemInstanceFactory
    {
        
        
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
