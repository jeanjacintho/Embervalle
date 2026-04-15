#nullable enable
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    /// <summary>Uma camada desenhável (sheet + tint); pode ser omitida (null sheet).</summary>
    public sealed class CharacterPart
    {
        public CharacterPartSlot Slot { get; init; }

        public SpriteSheet? Sheet { get; set; }

        public Color Tint { get; set; } = Color.White;

        public bool IsVisible { get; set; } = true;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        /// <summary>Instância em execução — índices na <see cref="Sheet"/> desta parte (cada parte pode ter sequências diferentes).</summary>
        public Animation? PlayingAnimation { get; set; }
    }
}
