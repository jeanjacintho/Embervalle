using Microsoft.Xna.Framework;

namespace Embervalle.Core.Sprites
{
    /// <summary>Quatro direções cardeais (visuais, combate, arma).</summary>
    public enum PlayerCardinalFacing
    {
        Down,
        Up,
        Left,
        Right,
    }

    /// <summary>Converte a direção lógica em vetor unidade 2D (usado no combate, ex. mira de arc).</summary>
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
