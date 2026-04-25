#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Sprites
{
    /// <summary>Instância desenhável: <see cref="SpriteSheet"/>, <see cref="Animation"/> ativa, origem e efeitos.</summary>
    public sealed class SpriteComponent
    {
        public SpriteSheet? Sheet { get; set; }
        public Animation? CurrentAnimation { get; private set; }
        public Vector2 Origin { get; set; } = SpriteOrigins.Character;
        public Color Tint { get; set; } = Color.White;
        public float Scale { get; set; } = 1f;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        /// <summary>Ativa a animação por nome; repõe a partir do template se mudou.</summary>
        public void SetAnimation(Animation template)
        {
            if (CurrentAnimation?.Name == template.Name)
            {
                return;
            }

            CurrentAnimation = template.CloneAndReset();
        }

        /// <summary>Propaga o delta à animação ativa.</summary>
        public void Update(float deltaTime) => CurrentAnimation?.Update(deltaTime);

        /// <summary>Região de recorte na folha do frame corrente, ou vazio se não houver folha/anim.</summary>
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
