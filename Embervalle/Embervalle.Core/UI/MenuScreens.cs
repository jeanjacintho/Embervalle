using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.UI
{
    public enum MainMenuHit
    {
        None,
        NewGame,
        Exit,
    }

    public enum PauseMenuHit
    {
        None,
        Continue,
        ExitToMainMenu,
    }

    /// <summary>
    /// Layout e desenho do menu principal e do painel de pausa (coordenadas de tela).
    /// </summary>
    public static class MenuScreens
    {
        private const int ButtonWidth = 320;
        private const int ButtonHeight = 56;
        private const int ButtonGap = 18;
        private const float LabelScale = 2f;

        public static MainMenuHit HitTestMainMenu(int viewportWidth, int viewportHeight, Point mouse)
        {
            LayoutMainMenu(viewportWidth, viewportHeight, out Rectangle newGame, out Rectangle exit);
            if (newGame.Contains(mouse))
            {
                return MainMenuHit.NewGame;
            }

            if (exit.Contains(mouse))
            {
                return MainMenuHit.Exit;
            }

            return MainMenuHit.None;
        }

        public static PauseMenuHit HitTestPauseMenu(int viewportWidth, int viewportHeight, Point mouse)
        {
            LayoutPauseMenu(viewportWidth, viewportHeight, out Rectangle cont, out Rectangle exit);
            if (cont.Contains(mouse))
            {
                return PauseMenuHit.Continue;
            }

            if (exit.Contains(mouse))
            {
                return PauseMenuHit.ExitToMainMenu;
            }

            return PauseMenuHit.None;
        }

        public static void DrawMainMenu(
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D pixel,
            int viewportWidth,
            int viewportHeight,
            Point mouse)
        {
            LayoutMainMenu(viewportWidth, viewportHeight, out Rectangle newGame, out Rectangle exit);

            DrawTitle(spriteBatch, font, viewportWidth, "Embervalle");

            DrawButton(spriteBatch, font, pixel, newGame, "New Game", mouse);
            DrawButton(spriteBatch, font, pixel, exit, "Exit", mouse);
        }

        public static void DrawPauseMenu(
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D pixel,
            int viewportWidth,
            int viewportHeight,
            Point mouse)
        {
            // Painel semi-opaco atrás dos botões
            int panelW = ButtonWidth + 80;
            int panelH = ButtonHeight * 2 + ButtonGap + 100;
            var panel = new Rectangle(
                (viewportWidth - panelW) / 2,
                (viewportHeight - panelH) / 2,
                panelW,
                panelH);
            spriteBatch.Draw(pixel, panel, new Color(0, 0, 0, 180));

            LayoutPauseMenu(viewportWidth, viewportHeight, out Rectangle cont, out Rectangle exit);
            DrawButton(spriteBatch, font, pixel, cont, "Continue", mouse);
            DrawButton(spriteBatch, font, pixel, exit, "Exit", mouse);
        }

        private static void LayoutMainMenu(int vw, int vh, out Rectangle newGame, out Rectangle exit)
        {
            int x = (vw - ButtonWidth) / 2;
            int centerY = vh / 2;
            newGame = new Rectangle(x, centerY - ButtonHeight - ButtonGap / 2, ButtonWidth, ButtonHeight);
            exit = new Rectangle(x, centerY + ButtonGap / 2, ButtonWidth, ButtonHeight);
        }

        private static void LayoutPauseMenu(int vw, int vh, out Rectangle cont, out Rectangle exit)
        {
            int x = (vw - ButtonWidth) / 2;
            int centerY = vh / 2;
            cont = new Rectangle(x, centerY - ButtonHeight - ButtonGap / 2, ButtonWidth, ButtonHeight);
            exit = new Rectangle(x, centerY + ButtonGap / 2, ButtonWidth, ButtonHeight);
        }

        private static void DrawTitle(SpriteBatch spriteBatch, SpriteFont font, int viewportWidth, string title)
        {
            Vector2 size = font.MeasureString(title) * LabelScale;
            var pos = new Vector2((viewportWidth - size.X) / 2f, 80f);
            spriteBatch.DrawString(font, title, pos, Color.White, 0f, Vector2.Zero, LabelScale, SpriteEffects.None, 0f);
        }

        private static void DrawButton(
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D pixel,
            Rectangle rect,
            string label,
            Point mouse)
        {
            bool hover = rect.Contains(mouse);
            Color back = hover ? new Color(70, 90, 120) : new Color(45, 55, 70);
            Color border = hover ? new Color(200, 200, 220) : new Color(120, 130, 150);

            spriteBatch.Draw(pixel, rect, back);
            DrawRectOutline(spriteBatch, pixel, rect, border, 2);

            Vector2 textSize = font.MeasureString(label) * LabelScale;
            var textPos = new Vector2(
                rect.X + (rect.Width - textSize.X) / 2f,
                rect.Y + (rect.Height - textSize.Y) / 2f);
            spriteBatch.DrawString(
                font,
                label,
                textPos,
                Color.White,
                0f,
                Vector2.Zero,
                LabelScale,
                SpriteEffects.None,
                0f);
        }

        private static void DrawRectOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color color, int thickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}
