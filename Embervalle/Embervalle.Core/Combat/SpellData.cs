namespace Embervalle.Core.Combat
{
    /// <summary>Dados de definição de um feitiço: tipo, dano, custo de mana, recarga e parâmetros de projétil ou área.</summary>
    public sealed class SpellData
    {
        public required string SpellId { get; init; }

        public SpellType Type { get; init; }

        public int Damage { get; init; }

        public float ManaCost { get; init; }

        public float Cooldown { get; init; }

        public float ProjectileSpeed { get; init; }

        public float ProjectileRange { get; init; }

        public float AreaRadius { get; init; }
    }
}
