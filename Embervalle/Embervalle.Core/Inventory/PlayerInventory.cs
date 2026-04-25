using Embervalle.Core.Events;

namespace Embervalle.Core.Inventory
{
    /// <summary>Inventário principal do jogador com 30 slots e controle de peso máximo.</summary>
    public sealed class PlayerInventory : IContainer
    {
        public const int MainSlots = 30;

        public const int TotalSlots = MainSlots;

        public string ContainerId => "player";

        private readonly ItemSlot[] _slots = new ItemSlot[TotalSlots];

        public float CurrentWeight { get; private set; }

        public float MaxWeight { get; set; } = 50f;

        public bool IsOverweight => CurrentWeight > MaxWeight;

        /// <summary>Inicializa os slots e deixa o peso a zero.</summary>
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

        /// <summary>Tenta adicionar o stack respeitando o limite de peso; devolve quantidade que não coube.</summary>
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

        /// <summary>Recalcula o peso total a partir dos slots e publica o evento de peso.</summary>
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

        /// <summary>Publica <see cref="InventoryWeightChangedEvent"/> com peso e limite atuais.</summary>
        private void UpdateWeightPublished()
        {
            EventBus.Publish(new InventoryWeightChangedEvent { Current = CurrentWeight, Max = MaxWeight });
        }
    }
}
