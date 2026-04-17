using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Inimigo ou alvo de treino — identificação por id estável (save, quests).</summary>
    public sealed class CombatEnemy
    {
        public int Id { get; init; }

        public Vector2 FeetPosition { get; set; }

        public HitboxComponent Hitbox { get; } = new();

        public HealthComponent Health { get; } = new();

        public StatusEffectComponent? Status { get; init; }

        public static CombatEnemy Create(int id, Vector2 feetPosition, int hitPoints)
        {
            var e = new CombatEnemy
            {
                Id = id,
                FeetPosition = feetPosition,
            };
            e.Health.Max = hitPoints;
            e.Health.Current = hitPoints;
            return e;
        }
    }
}
