namespace Embervalle.Core.Combat
{
    /// <summary>Dados estáticos de arma — tabela de design.</summary>
    public sealed class WeaponData
    {
        public required string Id { get; init; }

        public WeaponKind Kind { get; init; }

        public int Damage { get; init; }

        /// <summary>Alcance em pixels (melee = raio do arco; arco = não usado no WeaponData direto).</summary>
        public float Range { get; init; }

        /// <summary>Meio ângulo do cone em radianos (arco total = 2×).</summary>
        public float ArcHalfAngleRadians { get; init; }

        public float Knockback { get; init; }

        public float CooldownSeconds { get; init; }

        public float BowChargeSecondsMax { get; init; }

        public int BowBaseDamage { get; init; }

        public float BowProjectileSpeed { get; init; }

        public float BowMaxRange { get; init; }

        public float ChargeDamageBonusMax { get; init; }
    }
}
