#nullable enable
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    
    public sealed class CharacterPart
    {
        public CharacterPartSlot Slot { get; init; }

        public SpriteSheet? Sheet { get; set; }

        public Color Tint { get; set; } = Color.White;

        public bool IsVisible { get; set; } = true;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        
        public Animation? PlayingAnimation { get; set; }
    }
}
