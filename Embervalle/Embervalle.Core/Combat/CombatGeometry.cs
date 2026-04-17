using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public static class CombatGeometry
    {
        public static float AngleBetweenRadians(Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a, b);
            dot = MathHelper.Clamp(dot, -1f, 1f);
            return MathF.Acos(dot);
        }

        /// <summary>Interseção segmento de reta com retângulo alinhado aos eixos (swept collision).</summary>
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

        private static bool RectContainsPoint(Rectangle rect, Vector2 p) =>
            p.X >= rect.Left && p.X < rect.Right && p.Y >= rect.Top && p.Y < rect.Bottom;

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
