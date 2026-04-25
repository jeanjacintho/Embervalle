using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat.Hostile
{
    /// <summary>Regras de visão e audição; linha de visão plena via <see cref="HasLineOfSightSimple"/> até haver mapa de colisão.</summary>
    public static class EnemyPerception
    {
        /// <summary>Verifica distância, FOV, cone e (stub) linha de visão.</summary>
        /// <param name="facingDirectionNormalized">Direção frontal do inimigo (define o cone de FOV).</param>
        /// <param name="hasClearLineOfSight">Falso se houver parede sólida entre inimigo e jogador (hoje: stub).</param>
        public static bool IsPlayerVisible(
            Vector2 enemyFeet,
            Vector2 facingDirectionNormalized,
            Vector2 playerFeet,
            in HostileEnemyProfile profile,
            bool hasClearLineOfSight)
        {
            float dist = Vector2.Distance(enemyFeet, playerFeet);
            if (dist > profile.DetectionRange)
            {
                return false;
            }

            Vector2 toPlayer = playerFeet - enemyFeet;
            if (toPlayer.LengthSquared() < 0.25f)
            {
                return hasClearLineOfSight;
            }

            toPlayer /= MathF.Sqrt(toPlayer.LengthSquared());
            if (profile.FieldOfViewDegrees >= 359f)
            {
                return hasClearLineOfSight;
            }

            float halfFov = MathHelper.ToRadians(profile.FieldOfViewDegrees * 0.5f);
            float ang = AngleBetweenRadians(facingDirectionNormalized, toPlayer);
            if (ang > halfFov)
            {
                return false;
            }

            return hasClearLineOfSight;
        }

        /// <summary>Verifica se o jogador está dentro do alcance de audição, com penalização se não estiver a correr.</summary>
        /// <param name="playerIsRunning">Se verdadeiro, usa o alcance de audição completo do perfil.</param>
        public static bool IsPlayerHeard(
            Vector2 enemyFeet,
            Vector2 playerFeet,
            in HostileEnemyProfile profile,
            bool playerIsRunning)
        {
            float hearRange = playerIsRunning
                ? profile.HearingRange
                : profile.HearingRange * 0.4f;
            return Vector2.Distance(enemyFeet, playerFeet) < hearRange;
        }

        /// <summary>Substituir por raycast de tiles quando o mapa de combate estiver integrado.</summary>
        public static bool HasLineOfSightSimple(Vector2 fromFeet, Vector2 toFeet) => true;

        /// <summary>Ângulo entre duas direções (vetores) em radianos [0, π].</summary>
        public static float AngleBetweenRadians(Vector2 aDir, Vector2 bDir)
        {
            float al = aDir.Length();
            float bl = bDir.Length();
            if (al < 1e-4f)
            {
                al = 1f;
                aDir = Vector2.UnitY;
            }
            else
            {
                aDir /= al;
            }

            if (bl < 1e-4f)
            {
                bDir = Vector2.UnitY;
            }
            else
            {
                bDir /= bl;
            }

            float d = MathHelper.Clamp(Vector2.Dot(aDir, bDir), -1f, 1f);
            return MathF.Acos(d);
        }
    }
}
