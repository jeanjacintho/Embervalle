using Embervalle.Core.Events;

namespace Embervalle.Core.Combat
{
    /// <summary>Componente de efeito de estado ativo em um personagem, com duração e aplicação de dano periódico.</summary>
    public sealed class StatusEffectComponent
    {
        public StatusEffect Active { get; set; } = StatusEffect.None;

        public float Duration { get; set; }

        public float Intensity { get; set; } = 1f;

        /// <summary>Ativa o efeito, guarda duração/intensidade e publica <see cref="StatusAppliedEvent"/>.</summary>
        public void Apply(StatusEffect effect, float duration, float intensity = 1f)
        {
            Active = effect;
            Duration = duration;
            Intensity = intensity;
            EventBus.Publish(new StatusAppliedEvent { Effect = effect });
        }

        /// <summary>Atualiza duração, aplica dano por DoT (queimadura/veneno) e limpa quando expira.</summary>
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
