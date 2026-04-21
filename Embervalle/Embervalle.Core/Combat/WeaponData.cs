namespace Embervalle.Core.Combat
{
    
    public sealed class WeaponData
    {
        public required string Id { get; init; }

        public WeaponKind Kind { get; init; }

        public int Damage { get; init; }

        
        public float Range { get; init; }

        
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
