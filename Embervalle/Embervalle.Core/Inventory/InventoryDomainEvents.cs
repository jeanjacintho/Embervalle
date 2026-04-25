namespace Embervalle.Core.Inventory
{
    /// <summary>Evento publicado quando o peso atual do inventário do jogador muda.</summary>
    public sealed class InventoryWeightChangedEvent
    {
        public float Current { get; init; }

        public float Max { get; init; }
    }

    /// <summary>Evento publicado quando qualquer slot de um container é modificado.</summary>
    public sealed class ContainerChangedEvent
    {
        public string ContainerId { get; init; } = "";
    }

    /// <summary>Evento publicado quando um item é equipado em um slot de equipamento.</summary>
    public sealed class ItemEquippedEvent
    {
        public EquipmentSlotType SlotType { get; init; }

        public string? ItemId { get; init; }
    }

    /// <summary>Evento publicado quando um item perecível expira e apodrece em um container.</summary>
    public sealed class ItemSpoiledEvent
    {
        public string ContainerId { get; init; } = "";
    }
}
