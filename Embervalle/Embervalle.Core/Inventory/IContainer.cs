namespace Embervalle.Core.Inventory
{
    public interface IContainer
    {
        string ContainerId { get; }

        int SlotCount { get; }

        ItemSlot GetSlot(int index);

        ItemSlot[] GetAllSlots();

        
        int TryAdd(ItemInstance item);

        bool TryRemove(string itemId, int qty);

        bool Has(string itemId, int qty);

        int Count(string itemId);
    }
}
