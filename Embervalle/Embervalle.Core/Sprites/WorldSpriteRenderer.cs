using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Sprites
{
    /// <summary>Desenha entidades de sprite no mundo, profundidade por Y (ordem) e sombra de elipse simples.</summary>
    public sealed class WorldSpriteRenderer
    {
        private readonly Texture2D _pixel;
        private float _mapHeightPixels;

        public WorldSpriteRenderer(Texture2D singlePixelTexture, float mapHeightPixels)
        {
            _pixel = singlePixelTexture;
            _mapHeightPixels = MathHelper.Max(1f, mapHeightPixels);
        }

        public void SetMapHeight(float mapHeightPixels) => _mapHeightPixels = MathHelper.Max(1f, mapHeightPixels);

        public float GetLayerDepth(float feetWorldY) => MathHelper.Clamp(1f - feetWorldY / _mapHeightPixels, 0f, 1f);

        public void DrawEntity(SpriteBatch spriteBatch, SpriteComponent sprite, Vector2 feetWorldPosition)
        {
            if (sprite.CurrentAnimation == null || sprite.Sheet == null)
            {
                return;
            }

            float depth = GetLayerDepth(feetWorldPosition.Y);
            SpriteEffects effects = sprite.Effects;
            if (sprite.CurrentAnimation.FlipHorizontal)
            {
                effects |= SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(
                sprite.Sheet.Texture,
                feetWorldPosition,
                sprite.CurrentSourceRect,
                sprite.Tint,
                0f,
                sprite.Origin,
                sprite.Scale,
                effects,
                depth);
            DrawShadow(spriteBatch, feetWorldPosition, depth);
        }

        private void DrawShadow(SpriteBatch spriteBatch, Vector2 feet, float entityDepth)
        {
            float shadowDepth = MathHelper.Clamp(entityDepth - 0.02f, 0f, 1f);
            Vector2 shadowCenter = feet + new Vector2(0f, 2f);
            spriteBatch.Draw(
                _pixel,
                shadowCenter,
                null,
                Color.Black * 0.4f,
                0f,
                new Vector2(0.5f, 0.5f),
                new Vector2(20f, 6f),
                SpriteEffects.None,
                shadowDepth);
        }
    }
}
