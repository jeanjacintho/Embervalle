using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core.Gameplay
{
    /// <summary>Input WASD no corpo do jogador: desloca e aplica limites do viewport (origem do sprite).</summary>
    public static class PlayerWASDMovement
    {
        /// <summary>Posiciona os pés do jogador ao centro do viewport com velocidade zero.</summary>
        public static void SpawnCentered(PlayerBody body, int viewportWidth, int viewportHeight)
        {
            body.FeetPosition = new Vector2(viewportWidth / 2f, viewportHeight / 2f);
            body.LastVelocity = Vector2.Zero;
        }

        /// <summary>Aplica input WASD, atualiza <see cref="PlayerBody.FeetPosition"/> e <see cref="PlayerBody.LastVelocity"/>, e limita à área do ecrã.</summary>
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
                body.FeetPosition += move * body.MoveSpeedPixelsPerSecond * deltaSeconds;
                body.LastVelocity = move * body.MoveSpeedPixelsPerSecond;
            }
            else
            {
                body.LastVelocity = Vector2.Zero;
            }

            Vector2 o = SpriteOrigins.Character;
            float minX = o.X;
            float maxX = viewportWidth - (PlayerBody.VisualFrameWidth - o.X);
            float minY = o.Y;
            float maxY = viewportHeight - (PlayerBody.VisualFrameHeight - o.Y);
            body.FeetPosition.X = MathHelper.Clamp(body.FeetPosition.X, minX, MathHelper.Max(minX, maxX));
            body.FeetPosition.Y = MathHelper.Clamp(body.FeetPosition.Y, minY, MathHelper.Max(minY, maxY));
        }
    }
}
