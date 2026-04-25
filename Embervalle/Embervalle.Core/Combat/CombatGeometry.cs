using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Funções geométricas auxiliares para cálculos de colisão e ângulos no combate.</summary>
    public static class CombatGeometry
    {
        /// <summary>Ângulo em radianos entre dois vetores unitários (via produto interno limitado a [-1,1]).</summary>
        public static float AngleBetweenRadians(Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a, b);
            dot = MathHelper.Clamp(dot, -1f, 1f);
            return MathF.Acos(dot);
        }

        /// <summary>Indica se o segmento [start,end] intersecta o interior ou arestas do retângulo.</summary>
        public static bool SegmentIntersectsRect(Vector2 start, Vector2 end, Rectangle rect)
        {
            if (RectContainsPoint(rect, start) || RectContainsPoint(rect, end))
            {
                return true;
            }

            Vector2 min = new(rect.Left, rect.Top);
            Vector2 max = new(rect.Right, rect.Bottom);

            if (SegmentIntersectsHorizontalLine(start, end, min.X, min.Y, max.Y))
            {
                return true;
            }

            if (SegmentIntersectsHorizontalLine(start, end, max.X, min.Y, max.Y))
            {
                return true;
            }

            if (SegmentIntersectsVerticalLine(start, end, min.Y, min.X, max.X))
            {
                return true;
            }

            if (SegmentIntersectsVerticalLine(start, end, max.Y, min.X, max.X))
            {
                return true;
            }

            return false;
        }

        /// <summary>Testa inclusão de p no retângulo (semiaberto no canto direito/baixo como <see cref="Rectangle"/>).</summary>
        private static bool RectContainsPoint(Rectangle rect, Vector2 p) =>
            p.X >= rect.Left && p.X < rect.Right && p.Y >= rect.Top && p.Y < rect.Bottom;

        /// <summary>Interseção do segmento com a linha vertical x constante, clippada a [yMin,yMax].</summary>
        private static bool SegmentIntersectsHorizontalLine(
            Vector2 start,
            Vector2 end,
            float x,
            float yMin,
            float yMax)
        {
            float dx = end.X - start.X;
            if (MathF.Abs(dx) < 1e-6f)
            {
                return false;
            }

            float t = (x - start.X) / dx;
            if (t < 0f || t > 1f)
            {
                return false;
            }

            float y = start.Y + (end.Y - start.Y) * t;
            return y >= yMin && y <= yMax;
        }

        /// <summary>Interseção do segmento com a linha horizontal y constante, clippada a [xMin,xMax].</summary>
        private static bool SegmentIntersectsVerticalLine(
            Vector2 start,
            Vector2 end,
            float y,
            float xMin,
            float xMax)
        {
            float dy = end.Y - start.Y;
            if (MathF.Abs(dy) < 1e-6f)
            {
                return false;
            }

            float t = (y - start.Y) / dy;
            if (t < 0f || t > 1f)
            {
                return false;
            }

            float x = start.X + (end.X - start.X) * t;
            return x >= xMin && x <= xMax;
        }
    }
}
