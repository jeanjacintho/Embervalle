namespace Embervalle.Core.Inventory
{
    
    
    public sealed class ToolbarSlots : IContainer
    {
        public const int SlotCountValue = 2;

        public string ContainerId => "toolbar";

        private readonly ItemSlot[] _slots = new ItemSlot[SlotCountValue];

        public ToolbarSlots()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new ItemSlot();
            }
        }

        public int SlotCount => SlotCountValue;

        public ItemSlot GetSlot(int index) => _slots[index];

        public ItemSlot[] GetAllSlots() => _slots;

        public int TryAdd(ItemInstance item)
        {
            float w = 0f;
            return ContainerHelpers.TryAddToSlots(
                _slots,
                item,
                maxWeight: float.MaxValue,
                ref w);
        }

        public bool TryRemove(string itemId, int qty) =>
            ContainerHelpers.TryRemoveFromSlots(_slots, itemId, qty);

        public bool Has(string itemId, int qty) =>
            ContainerHelpers.HasInSlots(_slots, itemId, qty);

        public int Count(string itemId) => ContainerHelpers.CountInSlots(_slots, itemId);
    }
}
