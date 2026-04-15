using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Um NPC = definição de dados (doc 07).
    /// <see cref="VisualKind"/> escolhe entre corpo modular 4 partes ou uma sheet única.
    /// Com <see cref="CharacterVisualKind.CompositeFourPart"/>, <see cref="BodyType"/> liga ao <see cref="BodyTypeSpriteCatalog"/>.
    /// </summary>
    public sealed class CharacterDefinition
    {
        public string Id { get; init; } = "";

        public BodyType BodyType { get; init; } = BodyType.Average;

        public CharacterAppearance Appearance { get; init; } = new();

        /// <summary>Modular humanóide vs. sprite único (criaturas, estilos especiais).</summary>
        public CharacterVisualKind VisualKind { get; init; } = CharacterVisualKind.CompositeFourPart;

        /// <summary>Obrigatório se <see cref="VisualKind"/> == <see cref="CharacterVisualKind.SingleSpriteSheet"/> (vazio = usa sheet padrão do jogador).</summary>
        public string SingleSpriteContentPath { get; init; } = "";

        public int SingleSpriteFrameWidth { get; init; } = 48;

        public int SingleSpriteFrameHeight { get; init; } = 64;

        public Color? SingleSpriteTint { get; init; }
    }
}
