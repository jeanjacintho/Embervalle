using Microsoft.Xna.Framework;

namespace Embervalle.Core.Gameplay
{
    /// <summary>
    /// Dados do jogador para movimento e desenho (apenas estado — sem lógica).
    /// </summary>
    public sealed class PlayerBody
    {
        public Vector2 Position;

        public int Size { get; init; } = 32;

        public float MoveSpeedPixelsPerSecond { get; init; } = 220f;
    }
}
