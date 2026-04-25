using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>Biblioteca de animações padrão para personagens humanoides, mapeando IDs lógicos a templates de animação.</summary>
    public static class CharacterAnimationLibrary
    {
        /// <summary>Dicionário de animações padrão para personagens humanoides.</summary>
        public static IReadOnlyDictionary<CharacterAnimationId, Animation> DefaultHumanoid { get; } =
            new Dictionary<CharacterAnimationId, Animation>
            {
                [CharacterAnimationId.IdleDown] = PlayerAnimations.IdleDown,
                [CharacterAnimationId.IdleUp] = PlayerAnimations.IdleUp,
                [CharacterAnimationId.IdleLeft] = PlayerAnimations.IdleLeft,
                [CharacterAnimationId.IdleRight] = PlayerAnimations.IdleRight,

                [CharacterAnimationId.WalkDown] = PlayerAnimations.WalkDown,
                [CharacterAnimationId.WalkUp] = PlayerAnimations.WalkUp,
                [CharacterAnimationId.WalkLeft] = PlayerAnimations.WalkLeft,
                [CharacterAnimationId.WalkRight] = PlayerAnimations.WalkRight,

                [CharacterAnimationId.AttackDown] = PlayerAnimations.AttackDown,
                [CharacterAnimationId.AttackUp] = PlayerAnimations.AttackUp,
                [CharacterAnimationId.AttackLeft] = PlayerAnimations.AttackLeft,
                [CharacterAnimationId.AttackRight] = PlayerAnimations.AttackRight,

                [CharacterAnimationId.ToolUse] = PlayerAnimations.ToolUse,
                [CharacterAnimationId.Hurt] = PlayerAnimations.Hurt,
                [CharacterAnimationId.Death] = PlayerAnimations.Death,
            };

        /// <summary>Obtém a animação padrão para um ID de animação específico.</summary>
        public static Animation GetTemplate(CharacterAnimationId id)
        {
            return DefaultHumanoid.TryGetValue(id, out Animation? t)
                ? t
                : DefaultHumanoid[CharacterAnimationId.IdleDown];
        }
    }
}
