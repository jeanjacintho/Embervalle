namespace Embervalle.Core.Inventory
{
    /// <summary>Categorias que classificam o tipo de uso de um item.</summary>
    public enum ItemCategory
    {
        Weapon,
        Armor,
        Consumable,
        Resource,
        Food,
        Seed,
        Fish,
        QuestItem,
        Currency,
        Tool,
        Accessory,
    }

    /// <summary>Raridade de um item, de comum a lendário.</summary>
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    /// <summary>Qualidade de fabricação de um item, que influencia seus atributos.</summary>
    public enum ItemQuality
    {
        Normal,
        Good,
        Excellent,
        Mastercraft,
    }

    /// <summary>Slot de armadura no corpo do personagem.</summary>
    public enum ArmorSlot
    {
        Head,
        Chest,
        Legs,
        Feet,
    }

    /// <summary>Identificador de cada slot de equipamento disponível no personagem.</summary>
    public enum EquipmentSlotType
    {
        MainHand,
        OffHand,
        Head,
        Chest,
        Legs,
        Feet,
        Ring1,
        Ring2,
        Necklace,
        Tool,
    }

    /// <summary>Material ou finalidade de um baú, determina a capacidade de slots.</summary>
    public enum ChestType
    {
        Wood,
        Stone,
        Iron,
        LinkedAddress,
        ShopNpc,
    }

    /// <summary>Resultado de uma operação de transferência de item entre slots.</summary>
    public enum TransferResult
    {
        Empty,
        Blocked,
        Success,
        Swapped,
        Failed,
    }
}
