using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat.Hostile
{
    /// <summary>Utilitários compartilhados pela IA hostil (limites de arena).</summary>
    internal static class CombatEnemyAiShared
    {
        /// <summary>Mantém os pés do inimigo dentro do retângulo jogável, com base no hitbox.</summary>
        public static void ClampFeetToViewport(CombatEnemy e, int viewportWidth, int viewportHeight)
        {
            Vector2 o = e.Hitbox.OriginOffset;
            float w = e.Hitbox.Width;
            float h = e.Hitbox.Height;
            float minX = o.X;
            float maxX = viewportWidth - (w - o.X);
            float minY = o.Y;
            float maxY = viewportHeight - (h - o.Y);
            if (maxX < minX)
            {
                maxX = minX;
            }

            if (maxY < minY)
            {
                maxY = minY;
            }

            e.FeetPosition = new Vector2(
                MathHelper.Clamp(e.FeetPosition.X, minX, maxX),
                MathHelper.Clamp(e.FeetPosition.Y, minY, maxY));
        }
    }
}
