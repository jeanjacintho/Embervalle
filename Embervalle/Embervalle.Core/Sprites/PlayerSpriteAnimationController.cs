using Microsoft.Xna.Framework;

namespace Embervalle.Core.Sprites
{
    /// <summary>Máquina simples: idle / walk por direção; extensível para ataque e ferramenta.</summary>
    public sealed class PlayerSpriteAnimationController
    {
        private readonly SpriteComponent _sprite;

        private Direction _facing = Direction.Down;

        public PlayerSpriteAnimationController(SpriteComponent sprite)
        {
            _sprite = sprite;
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
                _sprite.SetAnimation(PlayerAnimations.ToolUse);
            }
            else if (isMoving)
            {
                SetWalkAnimation();
            }
            else
            {
                SetIdleAnimation();
            }

            _sprite.Update(deltaTime);
        }

        private void SetWalkAnimation()
        {
            _sprite.SetAnimation(
                _facing switch
                {
                    Direction.Down => PlayerAnimations.WalkDown,
                    Direction.Up => PlayerAnimations.WalkUp,
                    Direction.Left => PlayerAnimations.WalkLeft,
                    Direction.Right => PlayerAnimations.WalkRight,
                    _ => PlayerAnimations.WalkDown,
                });
        }

        private void SetIdleAnimation()
        {
            _sprite.SetAnimation(
                _facing switch
                {
                    Direction.Down => PlayerAnimations.IdleDown,
                    Direction.Up => PlayerAnimations.IdleUp,
                    Direction.Left => PlayerAnimations.IdleLeft,
                    Direction.Right => PlayerAnimations.IdleRight,
                    _ => PlayerAnimations.IdleDown,
                });
        }

        private void SetAttackAnimation()
        {
            _sprite.SetAnimation(
                _facing switch
                {
                    Direction.Down => PlayerAnimations.AttackDown,
                    Direction.Up => PlayerAnimations.AttackUp,
                    Direction.Left => PlayerAnimations.AttackLeft,
                    Direction.Right => PlayerAnimations.AttackRight,
                    _ => PlayerAnimations.AttackDown,
                });
        }
    }
}
