using Embervalle.Core.Events;

namespace Embervalle.Core.Inventory
{
    public sealed class PlayerInventory : IContainer
    {
        /// <summary>Grade da mochila 6x5 (sem hotbar estilo Minecraft).</summary>
        public const int MainSlots = 30;

        public const int TotalSlots = MainSlots;

        public string ContainerId => "player";

        private readonly ItemSlot[] _slots = new ItemSlot[TotalSlots];

        public float CurrentWeight { get; private set; }

        public float MaxWeight { get; set; } = 50f;

        public bool IsOverweight => CurrentWeight > MaxWeight;

        public PlayerInventory()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new ItemSlot();
            }
        }

        public int SlotCount => TotalSlots;

        public ItemSlot GetSlot(int index) => _slots[index];

        public ItemSlot[] GetAllSlots() => _slots;

        public int TryAdd(ItemInstance item)
        {
            UpdateWeight();
            float weightForLimit = CurrentWeight;
            int leftover = ContainerHelpers.TryAddToSlots(
                _slots,
                item,
                MaxWeight,
                ref weightForLimit);

            UpdateWeight();
            return leftover;
        }

        public bool TryRemove(string itemId, int qty) =>
            ContainerHelpers.TryRemoveFromSlots(_slots, itemId, qty);

        public bool Has(string itemId, int qty) =>
            ContainerHelpers.HasInSlots(_slots, itemId, qty);

        public int Count(string itemId) => ContainerHelpers.CountInSlots(_slots, itemId);

        public void UpdateWeight()
        {
            CurrentWeight = 0f;
            for (int i = 0; i < _slots.Length; i++)
            {
                ItemSlot slot = _slots[i];
                if (!slot.IsEmpty)
                {
                    ItemData data = slot.Item!.GetData();
                    CurrentWeight += data.Weight * slot.Item.Quantity;
                }
            }

            UpdateWeightPublished();
        }

        private void UpdateWeightPublished()
        {
            EventBus.Publish(new InventoryWeightChangedEvent { Current = CurrentWeight, Max = MaxWeight });
        }
    }
}
