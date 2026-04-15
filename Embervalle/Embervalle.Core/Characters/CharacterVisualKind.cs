namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Humanoide modular (4 partes) vs. uma única sheet — para NPCs que não seguem o sistema (criaturas, bosses, props animados).
    /// </summary>
    public enum CharacterVisualKind
    {
        /// <summary>Cabeça, torso, braços e pernas — ver <see cref="BodyTypeSpriteCatalog"/>.</summary>
        CompositeFourPart = 0,

        /// <summary>Corpo inteiro numa textura + mesmas animações lógicas (<see cref="CharacterAnimationId"/>).</summary>
        SingleSpriteSheet = 1,
    }
}
