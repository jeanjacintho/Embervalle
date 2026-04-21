using Embervalle.Core.Events;

namespace Embervalle.Core.Inventory
{
    
    public static class ItemFreshnessSystem
    {
        public static void TickFreshness(IContainer container)
        {
            ItemSlot[] slots = container.GetAllSlots();
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot slot = slots[i];
                if (slot.IsEmpty || !slot.Item!.IsPerishable)
                {
                    continue;
                }

                slot.Item.Freshness--;

                if (slot.Item.Freshness > 0)
                {
                    continue;
                }

                string spoiledId = slot.Item.ItemId + "_spoiled";
                if (ItemDatabase.Exists(spoiledId))
                {
                    slot.Item.ItemId = spoiledId;
                }
                else
                {
                    slot.Item = null;
                }

                EventBus.Publish(new ItemSpoiledEvent { ContainerId = container.ContainerId });
            }
        }
    }
}
