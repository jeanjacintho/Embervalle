using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core.Input
{
    /// <summary>Estado de teclado/rato lido uma vez por frame, com bordas (just pressed) para ações de combate e toolbar.</summary>
    public sealed class InputManager
    {
        public Vector2 MousePosition { get; private set; }
        public bool SelectQuickSlot0JustPressed { get; private set; }
        public bool SelectQuickSlot1JustPressed { get; private set; }
        public bool AttackHeld { get; private set; }
        public bool AttackJustPressed { get; private set; }
        public bool AttackJustReleased { get; private set; }
        public bool Spell1JustPressed { get; private set; }

        /// <summary>Atualiza posição do rato e bordas de teclas X, C, Q e botão esquerdo do rato (ataque).</summary>
        public void Update(
            KeyboardState keyboard,
            KeyboardState previousKeyboard,
            MouseState mouse,
            MouseState previousMouse)
        {
            MousePosition = mouse.Position.ToVector2();

            bool x = keyboard.IsKeyDown(Keys.X);
            bool xPrev = previousKeyboard.IsKeyDown(Keys.X);
            SelectQuickSlot0JustPressed = x && !xPrev;

            bool c = keyboard.IsKeyDown(Keys.C);
            bool cPrev = previousKeyboard.IsKeyDown(Keys.C);
            SelectQuickSlot1JustPressed = c && !cPrev;

            bool attackMouse = mouse.LeftButton == ButtonState.Pressed;
            bool attackMousePrev = previousMouse.LeftButton == ButtonState.Pressed;
            AttackHeld = attackMouse;
            AttackJustPressed = attackMouse && !attackMousePrev;
            AttackJustReleased = !attackMouse && attackMousePrev;

            bool spell1 = keyboard.IsKeyDown(Keys.Q);
            bool spell1Prev = previousKeyboard.IsKeyDown(Keys.Q);
            Spell1JustPressed = spell1 && !spell1Prev;
        }
    }
}
