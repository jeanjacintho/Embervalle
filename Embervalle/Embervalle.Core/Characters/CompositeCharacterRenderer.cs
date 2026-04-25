using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    /// <summary>Renderiza todas as camadas de um <see cref="CompositeCharacterComponent"/> com depth sorting correto.</summary>
    public sealed class CompositeCharacterRenderer
    {
        private readonly Texture2D _pixel;

        /// <summary>Usa a textura de 1x1 branca para desenhar a sombra elíptica.</summary>
        public CompositeCharacterRenderer(Texture2D singlePixelWhiteTexture)
        {
            _pixel = singlePixelWhiteTexture;
        }

        /// <summary>Desenha todas as partes do personagem (slot 0 a cabeça) com depth por camada.</summary>
        public void Draw(
            SpriteBatch spriteBatch,
            Vector2 feetWorldPosition,
            CompositeCharacterComponent composite,
            float baseLayerDepth)
        {
            for (int i = 0; i <= (int)CharacterPartSlot.Head; i++)
            {
                var slot = (CharacterPartSlot)i;
                DrawSlot(spriteBatch, feetWorldPosition, composite, slot, baseLayerDepth, i);
            }
        }

        /// <summary>Desenha um slot (parte ou sombra) com offset/escala da config e flip opcional.</summary>
        public void DrawSlot(
            SpriteBatch spriteBatch,
            Vector2 feetWorldPosition,
            CompositeCharacterComponent composite,
            CharacterPartSlot slot,
            float baseLayerDepth,
            int slotIndex)
        {
            BodyPartConfig cfg = composite.Config;
            CharacterPart part = composite.GetPart(slot);
            if (!part.IsVisible)
            {
                return;
            }

            float layerDepth = ComputePartDepth(baseLayerDepth, slotIndex);

            if (slot == CharacterPartSlot.Shadow)
            {
                DrawShadowEllipse(spriteBatch, feetWorldPosition, layerDepth);
                return;
            }

            if (part.Sheet == null || part.PlayingAnimation == null)
            {
                return;
            }

            Rectangle source = part.Sheet.GetFrame(part.PlayingAnimation.CurrentSheetFrameIndex);
            Vector2 offset = cfg.GetOffset(slot);
            Vector2 scale = cfg.GetScale(slot);
            SpriteEffects fx = part.Effects;
            if (composite.SpriteFlipHorizontal)
            {
                fx |= SpriteEffects.FlipHorizontally;
            }

            Vector2 drawPosition = feetWorldPosition + offset;
            spriteBatch.Draw(
                part.Sheet.Texture,
                drawPosition,
                source,
                part.Tint,
                0f,
                SpriteOrigins.Character,
                scale,
                fx,
                layerDepth);
        }

        /// <summary>Calcula depth ligeiramente diferente por índice de slot para ordenação correta.</summary>
        public static float ComputePartDepth(float baseLayerDepth, int slotIndex)
        {
            return MathHelper.Clamp(baseLayerDepth + 0.0015f - slotIndex * 0.0001f, 0f, 1f);
        }

        /// <summary>Desenha elipse de sombra aos pés do personagem.</summary>
        private void DrawShadowEllipse(SpriteBatch spriteBatch, Vector2 feet, float entityDepth)
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
