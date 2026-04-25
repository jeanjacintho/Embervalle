namespace Embervalle.Core.Inventory
{
    /// <summary>Popula o inventário e a barra de ação com os itens iniciais de uma nova partida.</summary>
    public static class NewGameInventoryBootstrap
    {
        
        
        public static void Apply(PlayerInventory inv, ToolbarSlots toolbar)
        {
            _ = inv.TryAdd(ItemInstanceFactory.Create("weapon_axe", 1));
            _ = inv.TryAdd(ItemInstanceFactory.Create("food_bread", 3));

            toolbar.GetSlot(0).Item = ItemInstanceFactory.Create("weapon_short_bow", 1);
            toolbar.GetSlot(1).Item = ItemInstanceFactory.Create("weapon_basic_sword", 1);
        }
    }
}
