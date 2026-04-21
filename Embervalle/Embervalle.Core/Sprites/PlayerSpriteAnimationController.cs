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

        /// <summary>Direção para combate / ferramentas (Stardew: movimento atual ou última direção em idle).</summary>
        public PlayerCardinalFacing GetCombatFacing(Vector2 velocity)
        {
            bool isMoving = velocity.LengthSquared() > 0.0001f;
            Direction d;
            if (isMoving)
            {
                const float axisEps = 0.0001f;
                if (velocity.X > axisEps || velocity.X < -axisEps)
                {
                    d = velocity.X > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    d = velocity.Y > 0 ? Direction.Down : Direction.Up;
                }
            }
            else
            {
                d = _facing;
            }

            return (PlayerCardinalFacing)(int)d;
        }

        public void Update(
            float deltaTime,
            Vector2 velocity,
            bool attacking,
            bool usingTool,
            Vector2? attackFaceDirection = null)
        {
            bool isMoving = velocity.LengthSquared() > 0.0001f;
            if (attacking && attackFaceDirection.HasValue)
            {
                Vector2 ad = attackFaceDirection.Value;
                if (ad.LengthSquared() > 0.0001f)
                {
                    ad = Vector2.Normalize(ad);
                    const float axisEps = 0.0001f;
                    if (ad.X > axisEps || ad.X < -axisEps)
                    {
                        _facing = ad.X > 0 ? Direction.Right : Direction.Left;
                    }
                    else
                    {
                        _facing = ad.Y > 0 ? Direction.Down : Direction.Up;
                    }
                }
            }
            else if (isMoving)
            {
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
