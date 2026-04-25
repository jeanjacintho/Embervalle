using Embervalle.Core.Combat.Hostile;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Alvo de combate: pés, hitbox, vida; inimigos hostis usam <see cref="HostileEnemyProfile"/> e <see cref="HostileEnemyState"/>.</summary>
    public sealed class CombatEnemy
    {
        public int Id { get; init; }
        public Vector2 FeetPosition { get; set; }
        public HitboxComponent Hitbox { get; } = new();
        public HealthComponent Health { get; } = new();
        public StatusEffectComponent? Status { get; init; }

        /// <summary>Quando falso, <see cref="Embervalle.Core.Combat.Hostile.HostileEnemyAiSystem"/> ignora a entidade.</summary>
        public bool IsHostileAi { get; set; }
        public HostileEnemyProfile HostileProfile { get; set; }
        public HostileEnemyState HostileState { get; set; } = HostileEnemyState.Patrol;
        /// <summary>Normalizado; eixo de cone de FOV e referência de ataque em arco.</summary>
        public Vector2 FacingDirection { get; set; } = new(0f, 1f);
        /// <summary>Atualizado quando o jogador é visto ou ouvido; usado em Alert/Search.</summary>
        public Vector2? LastKnownPlayerPosition { get; set; }
        /// <summary>Acumulado no Chase se não houver avistamento nem som.</summary>
        public float EngageLoseTimer { get; set; }
        public float AlertTimer { get; set; }
        public float SearchPhaseTimer { get; set; }
        public float PatrolWaitRemaining { get; set; }
        public int PatrolWaypointIndex { get; set; }
        public Vector2[] PatrolWorldWaypoints { get; set; } = System.Array.Empty<Vector2>();
        public float AttackCooldownRemaining { get; set; }

        /// <summary>Cria alvo simples (sem patrulha/IA hostil) com HP definido.</summary>
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

        /// <summary>Cria inimigo com perfil de IA, patrulha e FSM; se waypoints forem vazios, cria-se um segmento padrão.</summary>
        /// <param name="patrolWorldWaypoints">Caminho em loop; se vazio, gera-se um segmento à frente do spawn.</param>
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
