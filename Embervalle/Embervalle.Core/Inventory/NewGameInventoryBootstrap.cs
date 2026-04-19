namespace Embervalle.Core.Inventory
{
    public static class NewGameInventoryBootstrap
    {
        /// <summary>
        /// Inventario inicial: todos os itens vêm do catalogo (<see cref="ItemDatabase"/>).
        /// </summary>
        public static void Apply(PlayerInventory inv, QuickAccessSlots quick)
        {
            _ = inv.TryAdd(ItemInstanceFactory.Create("weapon_axe", 1));
            _ = inv.TryAdd(ItemInstanceFactory.Create("food_bread", 3));

            quick.GetSlot(0).Item = ItemInstanceFactory.Create("weapon_short_bow", 1);
            quick.GetSlot(1).Item = ItemInstanceFactory.Create("weapon_basic_sword", 1);
        }
    }
}
