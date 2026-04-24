using Embervalle.Core.Combat.Hostile;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    
    public sealed class CombatEnemy
    {
        public int Id { get; init; }

        public Vector2 FeetPosition { get; set; }

        public HitboxComponent Hitbox { get; } = new();

        public HealthComponent Health { get; } = new();

        public StatusEffectComponent? Status { get; init; }

        public bool IsHostileAi { get; set; }

        public HostileEnemyProfile HostileProfile { get; set; }

        public HostileEnemyState HostileState { get; set; } = HostileEnemyState.Patrol;

        public Vector2 FacingDirection { get; set; } = new(0f, 1f);

        public Vector2? LastKnownPlayerPosition { get; set; }

        public float EngageLoseTimer { get; set; }

        public float AlertTimer { get; set; }

        public float SearchPhaseTimer { get; set; }

        public float PatrolWaitRemaining { get; set; }

        public int PatrolWaypointIndex { get; set; }

        public Vector2[] PatrolWorldWaypoints { get; set; } = System.Array.Empty<Vector2>();

        public float AttackCooldownRemaining { get; set; }

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

        public static CombatEnemy CreateHostile(
            int id,
            Vector2 feetPosition,
            int hitPoints,
            in HostileEnemyProfile profile,
            Vector2[]? patrolWorldWaypoints)
        {
            Vector2[] w = patrolWorldWaypoints ?? System.Array.Empty<Vector2>();
            if (w.Length < 1)
            {
                w = new[]
                {
                    feetPosition,
                    feetPosition + new Vector2(100f, 0f),
                };
            }

            var e = new CombatEnemy
            {
                Id = id,
                FeetPosition = feetPosition,
                IsHostileAi = true,
                HostileProfile = profile,
                HostileState = HostileEnemyState.Patrol,
                PatrolWorldWaypoints = w,
                PatrolWaypointIndex = 0,
                FacingDirection = Vector2.UnitX,
            };
            e.Health.Max = hitPoints;
            e.Health.Current = hitPoints;
            return e;
        }
    }
}
