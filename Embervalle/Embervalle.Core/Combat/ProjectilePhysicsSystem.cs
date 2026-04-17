using System;
using System.Collections.Generic;
using Embervalle.Core.Events;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Movimento + swept collision no mesmo passo (evita tunneling).</summary>
    public sealed class ProjectilePhysicsSystem
    {
        private readonly ProjectilePool _pool;

        public ProjectilePhysicsSystem(ProjectilePool pool)
        {
            _pool = pool;
        }

        public void Update(IReadOnlyList<CombatEnemy> enemies, float deltaSeconds)
        {
            ReadOnlySpan<ProjectileState> slots = _pool.AllSlots;
            for (int i = 0; i < slots.Length; i++)
            {
                ProjectileState p = slots[i];
                if (!p.IsActive)
                {
                    continue;
                }

                Vector2 start = p.Position;

                p.Direction.Y += p.Gravity * deltaSeconds;
                float dirLen = p.Direction.Length();
                if (dirLen > 0.0001f)
                {
                    p.Direction /= dirLen;
                }

                Vector2 movement = p.Direction * p.Speed * deltaSeconds;
                Vector2 end = start + movement;

                CombatEnemy? hit = TryHitAlongSegment(start, end, enemies, p.OwnerId);
                if (hit != null)
                {
                    hit.Health.TakeDamage(p.Damage);
                    p.Reset();
                    continue;
                }

                p.Position = end;
                p.DistanceTraveled += movement.Length();

                if (p.DistanceTraveled >= p.MaxRange)
                {
                    p.Reset();
                    EventBus.Publish(new ArrowExpiredEvent { PoolIndex = i });
                }
            }
        }

        private static CombatEnemy? TryHitAlongSegment(
            Vector2 start,
            Vector2 end,
            IReadOnlyList<CombatEnemy> enemies,
            int projectileOwnerId)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                CombatEnemy e = enemies[i];
                if (e.Health.Current <= 0)
                {
                    continue;
                }

                if (projectileOwnerId == e.Id)
                {
                    continue;
                }

                Rectangle hb = e.Hitbox.GetRect(e.FeetPosition);
                if (CombatGeometry.SegmentIntersectsRect(start, end, hb))
                {
                    return e;
                }
            }

            return null;
        }
    }
}
