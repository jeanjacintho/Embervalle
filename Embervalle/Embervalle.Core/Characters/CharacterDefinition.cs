using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    
    
    public sealed class CharacterDefinition
    {
        public string Id { get; init; } = "";

        public BodyType BodyType { get; init; } = BodyType.Average;

        public CharacterAppearance Appearance { get; init; } = new();

        
        public CharacterVisualKind VisualKind { get; init; } = CharacterVisualKind.CompositeFourPart;

        
        public string SingleSpriteContentPath { get; init; } = "";

        public int SingleSpriteFrameWidth { get; init; } = 48;

        public int SingleSpriteFrameHeight { get; init; } = 64;

        public Color? SingleSpriteTint { get; init; }
    }
}
