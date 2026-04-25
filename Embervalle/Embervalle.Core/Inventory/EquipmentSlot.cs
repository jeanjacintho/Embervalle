namespace Embervalle.Core.Inventory
{
    /// <summary>Slot de equipamento tipado que valida se um item é compatível antes de aceitar.</summary>
    public sealed class EquipmentSlot
    {
        public EquipmentSlotType SlotType { get; }

        public ItemInstance? Item { get; set; }

        public EquipmentSlot(EquipmentSlotType slotType)
        {
            SlotType = slotType;
        }

        public bool CanAccept(ItemInstance? item)
        {
            if (item == null)
            {
                return false;
            }

            if (!ItemDatabase.TryGet(item.ItemId, out ItemData? data) || data is null)
            {
                return false;
            }

            return SlotType switch
            {
                EquipmentSlotType.MainHand => data.Category == ItemCategory.Weapon,
                EquipmentSlotType.OffHand => data.Category == ItemCategory.Weapon,
                EquipmentSlotType.Head => data.Category == ItemCategory.Armor
                    && data.Armor != null
                    && data.Armor.Slot == ArmorSlot.Head,
                EquipmentSlotType.Chest => data.Category == ItemCategory.Armor
                    && data.Armor != null
                    && data.Armor.Slot == ArmorSlot.Chest,
                EquipmentSlotType.Legs => data.Category == ItemCategory.Armor
                    && data.Armor != null
                    && data.Armor.Slot == ArmorSlot.Legs,
                EquipmentSlotType.Feet => data.Category == ItemCategory.Armor
                    && data.Armor != null
                    && data.Armor.Slot == ArmorSlot.Feet,
                EquipmentSlotType.Tool => data.Category == ItemCategory.Tool,
                EquipmentSlotType.Ring1 => data.Category == ItemCategory.Accessory,
                EquipmentSlotType.Ring2 => data.Category == ItemCategory.Accessory,
                EquipmentSlotType.Necklace => data.Category == ItemCategory.Accessory,
                _ => false,
            };
        }
    }
}
