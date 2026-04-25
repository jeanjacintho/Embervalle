using Embervalle.Core.Events;
using System;

namespace Embervalle.Core.Combat
{
    /// <summary>Componente de vida de um personagem: pontos atuais, máximos e timer de invencibilidade pós-dano.</summary>
    public sealed class HealthComponent
    {
        public int Current { get; set; }

        public int Max { get; set; }

        public float InvincibilityTimer { get; set; }

        public bool IsInvincible => InvincibilityTimer > 0f;

        /// <summary>Subtrai vida, opcionalmente inicia janela de invencibilidade e publica <see cref="DamageTakenEvent"/>.</summary>
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

        /// <summary>Decrementa o temporizador de invencibilidade após o frame.</summary>
        public void UpdateInvincibility(float deltaSeconds)
        {
            if (InvincibilityTimer > 0f)
            {
                InvincibilityTimer = Math.Max(0f, InvincibilityTimer - deltaSeconds);
            }
        }
    }
}
