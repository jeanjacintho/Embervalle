using Microsoft.Xna.Framework;

namespace Embervalle.Core.Sprites
{
    /// <summary>
    /// Direção em que o jogador está virado (idle / movimento), como em Stardew Valley —
    /// o uso de ferramenta / arma melee segue esta direção, não a mira do rato.
    /// </summary>
    public enum PlayerCardinalFacing
    {
        Down,
        Up,
        Left,
        Right,
    }

    /// <summary>Vetor unitário no plano do jogo (Y+ = sul no ecrã).</summary>
    public static class MeleeFacingVectors
    {
        public static Vector2 ToWorldUnit(PlayerCardinalFacing f) =>
            f switch
            {
                PlayerCardinalFacing.Left => new Vector2(-1f, 0f),
                PlayerCardinalFacing.Right => new Vector2(1f, 0f),
                PlayerCardinalFacing.Up => new Vector2(0f, -1f),
                PlayerCardinalFacing.Down => new Vector2(0f, 1f),
                _ => new Vector2(0f, 1f),
            };
    }
}
