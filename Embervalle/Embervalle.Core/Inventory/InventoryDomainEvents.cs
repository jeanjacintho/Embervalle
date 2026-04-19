namespace Embervalle.Core.Inventory
{
    public sealed class InventoryWeightChangedEvent
    {
        public float Current { get; init; }

        public float Max { get; init; }
    }

    public sealed class ContainerChangedEvent
    {
        public string ContainerId { get; init; } = "";
    }

    public sealed class ItemEquippedEvent
    {
        public EquipmentSlotType SlotType { get; init; }

        public string? ItemId { get; init; }
    }

    public sealed class ItemSpoiledEvent
    {
        public string ContainerId { get; init; } = "";
    }
}
