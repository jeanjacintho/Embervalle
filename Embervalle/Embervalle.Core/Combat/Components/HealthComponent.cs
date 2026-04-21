using Embervalle.Core.Events;
using System;

namespace Embervalle.Core.Combat
{
    public sealed class HealthComponent
    {
        public int Current { get; set; }

        public int Max { get; set; }

        public float InvincibilityTimer { get; set; }

        public bool IsInvincible => InvincibilityTimer > 0f;

        
        public void TakeDamage(int amount, float invincDuration = 0.5f, bool ignoreInvincibility = false)
        {
            if (!ignoreInvincibility && IsInvincible)
            {
                return;
            }

            Current = Math.Max(0, Current - amount);
            if (invincDuration > 0f)
            {
                InvincibilityTimer = invincDuration;
            }

            EventBus.Publish(new DamageTakenEvent { Amount = amount });
        }

        public void UpdateInvincibility(float deltaSeconds)
        {
            if (InvincibilityTimer > 0f)
            {
                InvincibilityTimer = Math.Max(0f, InvincibilityTimer - deltaSeconds);
            }
        }
    }
}
