using Embervalle.Core.Events;

namespace Embervalle.Core.Combat
{
    public sealed class StatusEffectComponent
    {
        public StatusEffect Active { get; set; } = StatusEffect.None;

        public float Duration { get; set; }

        public float Intensity { get; set; } = 1f;

        public void Apply(StatusEffect effect, float duration, float intensity = 1f)
        {
            Active = effect;
            Duration = duration;
            Intensity = intensity;
            EventBus.Publish(new StatusAppliedEvent { Effect = effect });
        }

        public void Update(HealthComponent health, float deltaSeconds)
        {
            if (Active == StatusEffect.None)
            {
                return;
            }

            Duration -= deltaSeconds;

            if (Active == StatusEffect.Burn)
            {
                health.TakeDamage((int)(5 * deltaSeconds), invincDuration: 0f, ignoreInvincibility: true);
            }
            else if (Active == StatusEffect.Poison)
            {
                health.TakeDamage((int)(2 * deltaSeconds), invincDuration: 0f, ignoreInvincibility: true);
            }

            if (Duration <= 0f)
            {
                Active = StatusEffect.None;
            }
        }
    }
}
