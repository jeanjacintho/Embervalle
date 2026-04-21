using Microsoft.Xna.Framework;

namespace Embervalle.Core.World
{
    
    
    public sealed class Camera2D
    {
        public Vector2 Offset { get; set; }

        public float Zoom { get; set; } = 1f;

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return (screenPosition / Zoom) + Offset;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return (worldPosition - Offset) * Zoom;
        }

        public Matrix GetTransformMatrix()
        {
            return Matrix.CreateTranslation(-Offset.X, -Offset.Y, 0f)
                * Matrix.CreateScale(Zoom, Zoom, 1f);
        }
    }
}
