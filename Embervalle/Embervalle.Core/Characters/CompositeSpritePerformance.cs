using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Utilitários de otimização de desempenho para personagens compostos: LOD de camadas e culling de animação.</summary>
    public static class CompositeSpritePerformance
    {
        /// <summary>Valor de cinzento neutro usado em heurísticas de LOD/cor.</summary>
        public const byte NeutralGray = 128;

        /// <summary>Devolve quantas partes do corpo desenhar consoante a distância à câmara (menos partes = mais longe).</summary>
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

        /// <summary>Indica se a animação do personagem deve atualizar (retângulo aproximado dos pés intersecta o viewport com margem).</summary>
        public static bool ShouldUpdateAnimation(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 100)
        {
            var bounds = new Rectangle(
                (int)(feetWorldPosition.X - marginPixels),
                (int)(feetWorldPosition.Y - marginPixels),
                248,
                264);
            return visibleViewport.Intersects(bounds);
        }

        /// <summary>Indica se o personagem entra no viewport para desenho (por defeito margem 64px).</summary>
        public static bool IsVisibleForDraw(Rectangle visibleViewport, Vector2 feetWorldPosition, int marginPixels = 64)
        {
            return ShouldUpdateAnimation(visibleViewport, feetWorldPosition, marginPixels);
        }
    }
}
