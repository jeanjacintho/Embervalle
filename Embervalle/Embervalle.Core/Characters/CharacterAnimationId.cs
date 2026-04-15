namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Ação/direção lógica partilhada por todas as camadas (cada <see cref="BodyPartSpriteSpec"/> define os índices na sua sheet).
    /// “Sul” = <see cref="WalkDown"/> / <see cref="IdleDown"/> (eixo Y+ no ecrã).
    /// </summary>
    public enum CharacterAnimationId
    {
        IdleDown,
        IdleUp,
        IdleLeft,
        IdleRight,

        WalkDown,
        WalkUp,
        WalkLeft,
        WalkRight,

        AttackDown,
        AttackUp,
        AttackLeft,
        AttackRight,

        ToolUse,
        Hurt,
        Death,
    }
}
