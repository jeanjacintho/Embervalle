using Microsoft.Xna.Framework;

namespace Embervalle.Core.Gameplay
{
    /// <summary>Corpo do jogador: posição dos pés, movimento, HP e janela de invencibilidade após dano.</summary>
    public sealed class PlayerBody
    {
        public const int VisualFrameWidth = 48;
        public const int VisualFrameHeight = 64;
        public Vector2 FeetPosition;
        public Vector2 LastVelocity { get; set; }
        public float MoveSpeedPixelsPerSecond { get; init; } = 220f;
        public float Health { get; set; } = 100f;
        public float MaxHealth { get; set; } = 100f;
        /// <summary>Contagem regressiva; enquanto &gt; 0, <see cref="TryApplyHurt"/> não aplica dano.</summary>
        public float HurtInvincibilityRemaining { get; set; }

        /// <summary>Reduz o temporizador de invencibilidade após dano.</summary>
        public void UpdateHurtInvincibility(float deltaSeconds)
        {
            if (HurtInvincibilityRemaining > 0f)
            {
                HurtInvincibilityRemaining = System.MathF.Max(0f, HurtInvincibilityRemaining - deltaSeconds);
            }
        }

        /// <summary>Reduz <see cref="Health"/>; inicia a invencibilidade; devolve falso se ainda houver <see cref="HurtInvincibilityRemaining"/>.</summary>
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
