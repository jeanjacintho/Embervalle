namespace Embervalle.Core.Inventory
{
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

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public enum ItemQuality
    {
        Normal,
        Good,
        Excellent,
        Mastercraft,
    }

    public enum ArmorSlot
    {
        Head,
        Chest,
        Legs,
        Feet,
    }

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

    public enum ChestType
    {
        Wood,
        Stone,
        Iron,
        LinkedAddress,
        ShopNpc,
    }

    public enum TransferResult
    {
        Empty,
        Blocked,
        Success,
        Swapped,
        Failed,
    }
}
