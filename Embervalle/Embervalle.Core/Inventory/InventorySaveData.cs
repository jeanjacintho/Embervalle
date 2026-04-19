using System.Collections.Generic;

namespace Embervalle.Core.Inventory
{
    /// <summary>Snapshot serializável — apenas primitivos e IDs (doc 14).</summary>
    public sealed class InventorySaveData
    {
        public List<SlotSaveData> PlayerSlots { get; set; } = new();

        public List<SlotSaveData> EquipmentSlots { get; set; } = new();

        public int ActiveHotbar { get; set; }

        public Dictionary<string, List<SlotSaveData>> Chests { get; set; } = new();
    }

    public sealed class SlotSaveData
    {
        public int SlotIndex { get; set; }

        public string? ItemId { get; set; }

        public int Quantity { get; set; }

        public int Freshness { get; set; }

        public string? Quality { get; set; }

        public string? Enchantment { get; set; }

        public int DurabilityCurrent { get; set; }
    }
}
