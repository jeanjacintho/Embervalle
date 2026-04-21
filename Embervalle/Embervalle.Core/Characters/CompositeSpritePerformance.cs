using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    
    public static class CompositeSpritePerformance
    {
        
        public const byte NeutralGray = 128;

        
        public static int GetLodPartCount(float distanceFromCameraPixels)
        {
            if (distanceFromCameraPixels < 200f)
            {
                return 5;
            }

            if (distanceFromCameraPixels < 400f)
            {
                return 3;
            }

            if (distanceFromCameraPixels < 600f)
            {
                return 2;
            }

            return 1;
        }

        
        public static bool ShouldUpdateAnimation(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 100)
        {
            var bounds = new Rectangle(
                (int)(feetWorldPosition.X - marginPixels),
                (int)(feetWorldPosition.Y - marginPixels),
                248,
                264);
            return visibleViewport.Intersects(bounds);
        }

        
        public static bool IsVisibleForDraw(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 64)
        {
            return ShouldUpdateAnimation(visibleViewport, feetWorldPosition, marginPixels);
        }
    }
}
