using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core.Gameplay
{
    /// <summary>
    /// Movimentação top-down com WASD e limites na viewport (lógica separada do loop do jogo).
    /// </summary>
    public static class PlayerWASDMovement
    {
        public static void SpawnCentered(PlayerBody body, int viewportWidth, int viewportHeight)
        {
            body.Position = new Vector2(
                (viewportWidth - body.Size) / 2f,
                (viewportHeight - body.Size) / 2f);
        }

        public static void Tick(
            PlayerBody body,
            KeyboardState keyboard,
            float deltaSeconds,
            int viewportWidth,
            int viewportHeight)
        {
            var move = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                move.Y -= 1f;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                move.Y += 1f;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                move.X -= 1f;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                move.X += 1f;
            }

            if (move != Vector2.Zero)
            {
                move.Normalize();
                body.Position += move * body.MoveSpeedPixelsPerSecond * deltaSeconds;
            }

            float maxX = viewportWidth - body.Size;
            float maxY = viewportHeight - body.Size;
            body.Position.X = MathHelper.Clamp(body.Position.X, 0f, MathHelper.Max(0f, maxX));
            body.Position.Y = MathHelper.Clamp(body.Position.Y, 0f, MathHelper.Max(0f, maxY));
        }
    }
}
