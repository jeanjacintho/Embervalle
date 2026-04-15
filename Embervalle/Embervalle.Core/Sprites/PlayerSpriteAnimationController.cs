using Embervalle.Core.Characters;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Sprites
{
    /// <summary>Idle / walk por direção — usa <see cref="CharacterAnimationId"/> + perfil do <see cref="BodyType"/> no componente.</summary>
    public sealed class PlayerSpriteAnimationController
    {
        private readonly ILocomotionAnimationTarget _target;

        private Direction _facing = Direction.Down;

        public PlayerSpriteAnimationController(ILocomotionAnimationTarget target)
        {
            _target = target;
        }

        private enum Direction
        {
            Down,
            Up,
            Left,
            Right,
        }

        public void Update(float deltaTime, Vector2 velocity, bool attacking, bool usingTool)
        {
            bool isMoving = velocity.LengthSquared() > 0.0001f;
            if (isMoving)
            {
                // Diagonal: priorizar animação lateral (esq./dir.); só cima/baixo se movimento puramente vertical.
                const float axisEps = 0.0001f;
                if (velocity.X > axisEps || velocity.X < -axisEps)
                {
                    _facing = velocity.X > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    _facing = velocity.Y > 0 ? Direction.Down : Direction.Up;
                }
            }

            if (attacking)
            {
                SetAttackAnimation();
            }
            else if (usingTool)
            {
                _target.SetLogicalAnimation(CharacterAnimationId.ToolUse);
            }
            else if (isMoving)
            {
                SetWalkAnimation();
            }
            else
            {
                SetIdleAnimation();
            }

            _target.UpdateAnimation(deltaTime);
        }

        private void SetWalkAnimation()
        {
            _target.SetLogicalAnimation(
                _facing switch
                {
                    Direction.Down => CharacterAnimationId.WalkDown,
                    Direction.Up => CharacterAnimationId.WalkUp,
                    Direction.Left => CharacterAnimationId.WalkLeft,
                    Direction.Right => CharacterAnimationId.WalkRight,
                    _ => CharacterAnimationId.WalkDown,
                });
        }

        private void SetIdleAnimation()
        {
            _target.SetLogicalAnimation(
                _facing switch
                {
                    Direction.Down => CharacterAnimationId.IdleDown,
                    Direction.Up => CharacterAnimationId.IdleUp,
                    Direction.Left => CharacterAnimationId.IdleLeft,
                    Direction.Right => CharacterAnimationId.IdleRight,
                    _ => CharacterAnimationId.IdleDown,
                });
        }

        private void SetAttackAnimation()
        {
            _target.SetLogicalAnimation(
                _facing switch
                {
                    Direction.Down => CharacterAnimationId.AttackDown,
                    Direction.Up => CharacterAnimationId.AttackUp,
                    Direction.Left => CharacterAnimationId.AttackLeft,
                    Direction.Right => CharacterAnimationId.AttackRight,
                    _ => CharacterAnimationId.AttackDown,
                });
        }
    }
}
