namespace Embervalle.Core.Combat
{
    public static class SpellDefinitions
    {
        public static readonly SpellData Fireball = new()
        {
            SpellId = "fireball",
            Type = SpellType.Projectile,
            Damage = 30,
            ManaCost = 15f,
            Cooldown = 0.6f,
            ProjectileSpeed = 420f,
            ProjectileRange = 400f,
            AreaRadius = 0f,
        };
    }
}
