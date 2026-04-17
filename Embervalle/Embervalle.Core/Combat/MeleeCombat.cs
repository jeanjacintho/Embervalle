using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public static class MeleeCombat
    {
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

                Vector2 enemyPos = enemy.FeetPosition;
                Vector2 toEnemy = enemyPos - origin;
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
