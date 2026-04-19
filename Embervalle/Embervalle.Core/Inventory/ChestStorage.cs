namespace Embervalle.Core.Inventory
{
    /// <summary>Baú ou container fixo no mundo (mesma estrutura de slots que o inventário).</summary>
    public sealed class ChestStorage : IContainer
    {
        public string ContainerId { get; }

        public ChestType Type { get; }

        public string? CustomName { get; set; }

        private readonly ItemSlot[] _slots;

        public bool IsLocked { get; set; }

        public string? RequiredKeyId { get; set; }

        public string? MapId { get; set; }

        public int TileX { get; set; }

        public int TileY { get; set; }

        public bool HasBeenOpened { get; set; }

        public string? LootTableId { get; set; }

        public ChestStorage(string containerId, ChestType type)
        {
            ContainerId = containerId;
            Type = type;
            int slotCount = type switch
            {
                ChestType.Wood => 18,
                ChestType.Stone => 27,
                ChestType.Iron => 36,
                ChestType.LinkedAddress => 27,
                ChestType.ShopNpc => 12,
                _ => 18,
            };

            _slots = new ItemSlot[slotCount];
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new ItemSlot();
            }
        }

        public int SlotCount => _slots.Length;

        public ItemSlot GetSlot(int index) => _slots[index];

        public ItemSlot[] GetAllSlots() => _slots;

        public int TryAdd(ItemInstance item)
        {
            float weight = 0f;
            return ContainerHelpers.TryAddToSlots(
                _slots,
                item,
                maxWeight: float.MaxValue,
                ref weight);
        }

        public bool TryRemove(string itemId, int qty) =>
            ContainerHelpers.TryRemoveFromSlots(_slots, itemId, qty);

        public bool Has(string itemId, int qty) =>
            ContainerHelpers.HasInSlots(_slots, itemId, qty);

        public int Count(string itemId) => ContainerHelpers.CountInSlots(_slots, itemId);
    }
}
