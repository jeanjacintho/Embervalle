using System.Collections.Generic;

namespace Embervalle.Core.Inventory
{
    /// <summary>Dados serializáveis de todo o estado do inventário para salvar e carregar partidas.</summary>
    public sealed class InventorySaveData
    {
        public List<SlotSaveData> PlayerSlots { get; set; } = new();

        public List<SlotSaveData> EquipmentSlots { get; set; } = new();

        public int ActiveHotbar { get; set; }

        public Dictionary<string, List<SlotSaveData>> Chests { get; set; } = new();
    }

    /// <summary>Dados serializáveis de um único slot de inventário, incluindo item e quantidade.</summary>
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
