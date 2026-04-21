using Microsoft.Xna.Framework;

namespace Embervalle.Core.Sprites
{
    
    
    public enum PlayerCardinalFacing
    {
        Down,
        Up,
        Left,
        Right,
    }

    
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
