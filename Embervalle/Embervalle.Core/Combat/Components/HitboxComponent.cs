using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    
    
    public sealed class HitboxComponent
    {
        public int Width { get; init; } = 32;

        public int Height { get; init; } = 48;

        public Vector2 OriginOffset { get; init; } = new(16f, 40f);

        public Rectangle GetRect(Vector2 feetWorldPosition)
        {
            float left = feetWorldPosition.X - OriginOffset.X;
            float top = feetWorldPosition.Y - OriginOffset.Y;
            return new Rectangle((int)MathF.Round(left), (int)MathF.Round(top), Width, Height);
        }
    }
}
