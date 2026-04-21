using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public static class MeleeCombat
    {
        /// <summary>
        /// Ataque de área para feitiços melee (arco + alcance, sem frame table).
        /// Usado por <see cref="SpellCastSystem"/> para AreaMelee.
        /// </summary>
        public static void ExecuteMeleeAttack(
            Vector2 origin,
            Vector2 aimDirNormalized,
            WeaponData weapon,
            IReadOnlyList<CombatEnemy> enemies)
        {
            if (aimDirNormalized.LengthSquared() < 0.0001f)
            {
                aimDirNormalized = Vector2.UnitX;
            }
            else
            {
                aimDirNormalized = Vector2.Normalize(aimDirNormalized);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                CombatEnemy enemy = enemies[i];
                if (enemy.Health.Current <= 0)
                {
                    continue;
                }

                Vector2 toEnemy = enemy.FeetPosition - origin;
                float dist = toEnemy.Length();
                if (dist > weapon.Range || dist < 1e-3f)
                {
                    continue;
                }

                Vector2 toNorm = toEnemy / dist;
                float angle = CombatGeometry.AngleBetweenRadians(aimDirNormalized, toNorm);
                if (angle > weapon.ArcHalfAngleRadians)
                {
                    continue;
                }

                enemy.Health.TakeDamage(weapon.Damage);
                ApplyKnockback(enemy, aimDirNormalized, weapon.Knockback);
            }
        }

        /// <summary>
        /// Verifica e aplica dano do frame atual do swing contra todos os inimigos.
        /// Padrão SDV: hitbox retangular por frame, inimigos com I-frames não são atingidos duas vezes.
        /// </summary>
        public static void ApplyFrameHit(
            Rectangle hitbox,
            WeaponData weapon,
            IReadOnlyList<CombatEnemy> enemies,
            HashSet<int> alreadyHitThisSwing)
        {
            Vector2 hitboxCenter = new Vector2(hitbox.Center.X, hitbox.Center.Y);

            for (int i = 0; i < enemies.Count; i++)
            {
                CombatEnemy enemy = enemies[i];
                if (enemy.Health.Current <= 0)
                {
                    continue;
                }

                if (alreadyHitThisSwing.Contains(enemy.Id))
                {
                    continue;
                }

                Rectangle enemyRect = enemy.Hitbox.GetRect(enemy.FeetPosition);
                if (!hitbox.Intersects(enemyRect))
                {
                    continue;
                }

                alreadyHitThisSwing.Add(enemy.Id);
                enemy.Health.TakeDamage(weapon.Damage);

                Vector2 toEnemy = enemy.FeetPosition - hitboxCenter;
                Vector2 dir = toEnemy.LengthSquared() > 0.0001f
                    ? Vector2.Normalize(toEnemy)
                    : Vector2.UnitX;
                ApplyKnockback(enemy, dir, weapon.Knockback);
            }
        }

        private static void ApplyKnockback(CombatEnemy enemy, Vector2 aimDir, float knockback)
        {
            if (knockback <= 0f)
            {
                return;
            }

            enemy.FeetPosition += aimDir * knockback;
        }
    }
}
