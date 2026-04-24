using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat.Hostile
{
    
    public static class EnemyPerception
    {
        
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

        public static bool HasLineOfSightSimple(Vector2 fromFeet, Vector2 toFeet) =>
            true;

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
