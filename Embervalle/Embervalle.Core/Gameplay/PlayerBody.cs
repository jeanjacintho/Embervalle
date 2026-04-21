using Microsoft.Xna.Framework;

namespace Embervalle.Core.Gameplay
{
    
    
    public sealed class PlayerBody
    {
        public const int VisualFrameWidth = 48;

        public const int VisualFrameHeight = 64;

        public Vector2 FeetPosition;

        public Vector2 LastVelocity { get; set; }

        public float MoveSpeedPixelsPerSecond { get; init; } = 220f;

        public float Health { get; set; } = 100f;

        public float MaxHealth { get; set; } = 100f;
    }
}
