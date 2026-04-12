using Microsoft.Xna.Framework;

namespace Embervalle.Core.Gameplay
{
    /// <summary>
    /// Estado do jogador: posição dos pés no mundo (ancoragem do sprite 48×64).
    /// Hitbox de colisão não é o frame inteiro — ver documentação de sprites.
    /// </summary>
    public sealed class PlayerBody
    {
        public const int VisualFrameWidth = 48;

        public const int VisualFrameHeight = 64;

        public Vector2 FeetPosition;

        public Vector2 LastVelocity { get; set; }

        public float MoveSpeedPixelsPerSecond { get; init; } = 220f;
    }
}
