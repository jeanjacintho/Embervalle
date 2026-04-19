using System.Collections.Generic;

namespace Embervalle.Core.Inventory
{
    /// <summary>Slots tipados de equipamento (Skyrim-like) — dados, não lógica de UI.</summary>
    public sealed class GearEquipmentSlots
    {
        public EquipmentSlot MainHand { get; } = new(EquipmentSlotType.MainHand);

        public EquipmentSlot OffHand { get; } = new(EquipmentSlotType.OffHand);

        public EquipmentSlot Head { get; } = new(EquipmentSlotType.Head);

        public EquipmentSlot Chest { get; } = new(EquipmentSlotType.Chest);

        public EquipmentSlot Legs { get; } = new(EquipmentSlotType.Legs);

        public EquipmentSlot Feet { get; } = new(EquipmentSlotType.Feet);

        public EquipmentSlot Ring1 { get; } = new(EquipmentSlotType.Ring1);

        public EquipmentSlot Ring2 { get; } = new(EquipmentSlotType.Ring2);

        public EquipmentSlot Necklace { get; } = new(EquipmentSlotType.Necklace);

        public EquipmentSlot Tool { get; } = new(EquipmentSlotType.Tool);

        public EquipmentSlot GetSlot(EquipmentSlotType type) =>
            type switch
            {
                EquipmentSlotType.MainHand => MainHand,
                EquipmentSlotType.OffHand => OffHand,
                EquipmentSlotType.Head => Head,
                EquipmentSlotType.Chest => Chest,
                EquipmentSlotType.Legs => Legs,
                EquipmentSlotType.Feet => Feet,
                EquipmentSlotType.Ring1 => Ring1,
                EquipmentSlotType.Ring2 => Ring2,
                EquipmentSlotType.Necklace => Necklace,
                EquipmentSlotType.Tool => Tool,
                _ => MainHand,
            };

        public IEnumerable<EquipmentSlot> AllSlots()
        {
            yield return MainHand;
            yield return OffHand;
            yield return Head;
            yield return Chest;
            yield return Legs;
            yield return Feet;
            yield return Ring1;
            yield return Ring2;
            yield return Necklace;
            yield return Tool;
        }
    }
}
