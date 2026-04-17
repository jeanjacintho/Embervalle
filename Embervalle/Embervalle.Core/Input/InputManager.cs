using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core.Input
{
    /// <summary>
    /// Input de combate para desktop — teclado + mouse, uma leitura por frame com edge detection.
    /// </summary>
    public sealed class InputManager
    {
        public Vector2 MousePosition { get; private set; }

        public bool AttackHeld { get; private set; }

        public bool AttackJustPressed { get; private set; }

        public bool AttackJustReleased { get; private set; }

        public bool Spell1JustPressed { get; private set; }

        public void Update(
            KeyboardState keyboard,
            KeyboardState previousKeyboard,
            MouseState mouse,
            MouseState previousMouse)
        {
            MousePosition = mouse.Position.ToVector2();

            bool attackKey = keyboard.IsKeyDown(Keys.Space);
            bool attackKeyPrev = previousKeyboard.IsKeyDown(Keys.Space);
            bool attackMouse = mouse.LeftButton == ButtonState.Pressed;
            bool attackMousePrev = previousMouse.LeftButton == ButtonState.Pressed;

            AttackHeld = attackKey || attackMouse;
            bool heldPrev = attackKeyPrev || attackMousePrev;
            AttackJustPressed = AttackHeld && !heldPrev;
            AttackJustReleased = !AttackHeld && heldPrev;

            bool spell1 = keyboard.IsKeyDown(Keys.Q);
            bool spell1Prev = previousKeyboard.IsKeyDown(Keys.Q);
            Spell1JustPressed = spell1 && !spell1Prev;
        }
    }
}
