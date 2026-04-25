#nullable enable
using Embervalle.Core.Assets;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Cria componentes de sprite único para personagens definidos com <see cref="CharacterVisualKind.SingleSpriteSheet"/>.</summary>
    public static class SpriteCharacterFactory
    {
        private const string DefaultPlayerSheetPath = "Sprites/Characters/player";

        /// <summary>Cria um <see cref="SpriteComponent"/> de folha única com animação idle inicial.</summary>
        public static SpriteComponent CreateSingleSprite(CharacterDefinition definition, AssetManager assetManager)
        {
            if (definition.VisualKind != CharacterVisualKind.SingleSpriteSheet)
            {
                throw new System.InvalidOperationException(
                    "VisualKind tem de ser SingleSpriteSheet — usa CompositeCharacterFactory para 4 partes.");
            }

            string path = string.IsNullOrWhiteSpace(definition.SingleSpriteContentPath)
                ? DefaultPlayerSheetPath
                : definition.SingleSpriteContentPath.Trim();

            SpriteSheet sheet = assetManager.LoadSheet(
                path,
                definition.SingleSpriteFrameWidth,
                definition.SingleSpriteFrameHeight);

            var sprite = new SpriteComponent
            {
                Sheet = sheet,
                Origin = SpriteOrigins.Character,
                Tint = definition.SingleSpriteTint ?? Color.White,
            };

            sprite.SetAnimation(PlayerAnimations.IdleDown);
            return sprite;
        }
    }
}
