namespace Embervalle.Core.Combat
{
    /// <summary>Componente de equipamento de um personagem NPC: arma principal e até três slots de feitiço.</summary>
    public sealed class EquipmentComponent
    {
        public WeaponData MainHand { get; set; } = WeaponDefinitions.BasicSword;

        public SpellData? SpellSlot1 { get; set; }

        public SpellData? SpellSlot2 { get; set; }

        public SpellData? SpellSlot3 { get; set; }
    }
}
