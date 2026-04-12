#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Sprites
{
    public sealed class SpriteComponent
    {
        public SpriteSheet? Sheet { get; set; }

        public Animation? CurrentAnimation { get; private set; }

        public Vector2 Origin { get; set; } = SpriteOrigins.Character;

        public Color Tint { get; set; } = Color.White;

        public float Scale { get; set; } = 1f;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public void SetAnimation(Animation template)
        {
            if (CurrentAnimation?.Name == template.Name)
            {
                return;
            }

            CurrentAnimation = template.CloneAndReset();
        }

        public void Update(float deltaTime)
        {
            CurrentAnimation?.Update(deltaTime);
        }

        public Rectangle CurrentSourceRect
        {
            get
            {
                if (Sheet == null || CurrentAnimation == null)
                {
                    return Rectangle.Empty;
                }

                return Sheet.GetFrame(CurrentAnimation.CurrentSheetFrameIndex);
            }
        }
    }
}
