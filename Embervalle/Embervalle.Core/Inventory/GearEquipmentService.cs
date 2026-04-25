using Embervalle.Core.Events;

namespace Embervalle.Core.Inventory
{
    /// <summary>Serviço estático para equipar e desequipar itens entre o inventário e os slots de equipamento.</summary>
    public static class GearEquipmentService
    {
        public static bool EquipFromInventory(
            PlayerInventory inv,
            GearEquipmentSlots gear,
            int inventorySlotIndex,
            EquipmentSlotType target)
        {
            EquipmentSlot equipSlot = gear.GetSlot(target);
            ItemSlot invSlot = inv.GetSlot(inventorySlotIndex);
            if (invSlot.IsEmpty || !equipSlot.CanAccept(invSlot.Item))
            {
                return false;
            }

            if (equipSlot.Item != null)
            {
                ItemInstance? tmp = equipSlot.Item;
                equipSlot.Item = invSlot.Item;
                invSlot.Item = tmp;
            }
            else
            {
                equipSlot.Item = invSlot.Item;
                invSlot.Item = null;
            }

            inv.UpdateWeight();
            EventBus.Publish(new ItemEquippedEvent { SlotType = target, ItemId = equipSlot.Item?.ItemId });
            return true;
        }

        public static bool UnequipToInventory(PlayerInventory inv, GearEquipmentSlots gear, EquipmentSlotType target)
        {
            EquipmentSlot equipSlot = gear.GetSlot(target);
            if (equipSlot.Item == null)
            {
                return false;
            }

            ItemInstance payload = equipSlot.Item;
            int leftover = inv.TryAdd(payload);
            if (leftover > 0)
            {
                return false;
            }

            equipSlot.Item = null;
            inv.UpdateWeight();
            return true;
        }
    }
}
