using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Culling, LOD e constantes de tint neutro (docs 07 e 09).</summary>
    public static class CompositeSpritePerformance
    {
        /// <summary>Cinza neutro para multiplicar tint e preservar cor desejada (doc 07).</summary>
        public const byte NeutralGray = 128;

        /// <summary>
        /// Quantas camadas desenhar conforme distância à câmera (doc 09).
        /// </summary>
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

        /// <summary>Não atualizar animação fora da vista (margem anti pop-in, doc 09).</summary>
        public static bool ShouldUpdateAnimation(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 100)
        {
            var bounds = new Rectangle(
                (int)(feetWorldPosition.X - marginPixels),
                (int)(feetWorldPosition.Y - marginPixels),
                248,
                264);
            return visibleViewport.Intersects(bounds);
        }

        /// <summary>Não desenhar personagem totalmente fora do ecrã (culling básico).</summary>
        public static bool IsVisibleForDraw(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 64)
        {
            return ShouldUpdateAnimation(visibleViewport, feetWorldPosition, marginPixels);
        }
    }
}
