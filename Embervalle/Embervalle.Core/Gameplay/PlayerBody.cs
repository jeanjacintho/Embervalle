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

        public float HurtInvincibilityRemaining { get; set; }

        public void UpdateHurtInvincibility(float deltaSeconds)
        {
            if (HurtInvincibilityRemaining > 0f)
            {
                HurtInvincibilityRemaining = System.MathF.Max(0f, HurtInvincibilityRemaining - deltaSeconds);
            }
        }

        public bool TryApplyHurt(int damage, float invincibilitySeconds)
        {
            if (HurtInvincibilityRemaining > 0f)
            {
                return false;
            }

            Health = System.MathF.Max(0f, Health - damage);
            HurtInvincibilityRemaining = invincibilitySeconds;
            return true;
        }
    }
}
