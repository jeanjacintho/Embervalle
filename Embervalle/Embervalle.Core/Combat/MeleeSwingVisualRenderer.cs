using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Combat
{
    
    
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
