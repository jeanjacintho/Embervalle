using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Combat
{
    /// <summary>
    /// Desenha o sprite da arma durante o swing melee, usando frames discretos —
    /// a mesma abordagem de <c>MeleeWeapon.drawDuringUse</c> no Stardew Valley.
    /// Cada frame tem posição e rotação pré-calculadas em <see cref="MeleeSwingFrameTable"/>.
    /// </summary>
    public static class MeleeSwingVisualRenderer
    {
        public static void Draw(
            SpriteBatch spriteBatch,
            SpriteSheet weaponSheet,
            int weaponIconFrameIndex,
            Vector2 feetWorldPosition,
            PlayerCardinalFacing facing,
            int frame,
            float baseLayerDepth,
            Color tint)
        {
            MeleeSwingFrameTable.FrameData fd = MeleeSwingFrameTable.Get(facing, frame);

            Rectangle src = weaponSheet.GetFrame(weaponIconFrameIndex);

            // Origem no cabo da arma (parte inferior do sprite, igual SDV center=(1,15)/16px)
            var origin = new Vector2(src.Width * 0.5f, src.Height * 0.82f);

            Vector2 pos = feetWorldPosition + fd.Offset;
            float depth = MathHelper.Clamp(baseLayerDepth + 0.001f, 0f, 1f);
            var scale = new Vector2(WeaponVisualConstants.DrawScale, WeaponVisualConstants.DrawScale);

            spriteBatch.Draw(
                weaponSheet.Texture,
                position: pos,
                sourceRectangle: src,
                color: tint,
                rotation: fd.Rotation,
                origin: origin,
                scale: scale,
                effects: fd.Fx,
                layerDepth: depth);
        }
    }
}
