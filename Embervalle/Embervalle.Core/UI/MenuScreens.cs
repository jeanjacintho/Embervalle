using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.UI
{
    /// <summary>Elemento clicado no menu principal.</summary>
    public enum MainMenuHit
    {
        None,
        NewGame,
        Exit,
    }

    /// <summary>Elemento clicado no menu de pausa.</summary>
    public enum PauseMenuHit
    {
        None,
        Continue,
        ExitToMainMenu,
    }

    /// <summary>Renderiza e faz hit-test nos menus principal e de pausa do jogo.</summary>
    public static class MenuScreens
    {
        private const int ButtonWidth = 320;
        private const int ButtonHeight = 56;
        private const int ButtonGap = 18;
        private const float LabelScale = 2f;

        /// <summary>Devolve o botão do menu principal sob o rato, ou <see cref="MainMenuHit.None"/>.</summary>
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

        /// <summary>Devolve o botão do menu de pausa sob o rato, ou <see cref="PauseMenuHit.None"/>.</summary>
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

        /// <summary>Desenha o ecrã do menu principal (título e botões).</summary>
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

        /// <summary>Desenha o painel semitransparente e os botões do menu de pausa.</summary>
        public static void DrawPauseMenu(
            SpriteBatch spriteBatch,
            SpriteFont font,
            Texture2D pixel,
            int viewportWidth,
            int viewportHeight,
            Point mouse)
        {
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

        /// <summary>Calcula retângulos de New Game e Exit centrados no viewport.</summary>
        private static void LayoutMainMenu(int vw, int vh, out Rectangle newGame, out Rectangle exit)
        {
            int x = (vw - ButtonWidth) / 2;
            int centerY = vh / 2;
            newGame = new Rectangle(x, centerY - ButtonHeight - ButtonGap / 2, ButtonWidth, ButtonHeight);
            exit = new Rectangle(x, centerY + ButtonGap / 2, ButtonWidth, ButtonHeight);
        }

        /// <summary>Calcula retângulos de Continue e Exit ao centro do ecrã.</summary>
        private static void LayoutPauseMenu(int vw, int vh, out Rectangle cont, out Rectangle exit)
        {
            int x = (vw - ButtonWidth) / 2;
            int centerY = vh / 2;
            cont = new Rectangle(x, centerY - ButtonHeight - ButtonGap / 2, ButtonWidth, ButtonHeight);
            exit = new Rectangle(x, centerY + ButtonGap / 2, ButtonWidth, ButtonHeight);
        }

        /// <summary>Desenha o título escalado no topo do ecrã.</summary>
        private static void DrawTitle(SpriteBatch spriteBatch, SpriteFont font, int viewportWidth, string title)
        {
            Vector2 size = font.MeasureString(title) * LabelScale;
            var pos = new Vector2((viewportWidth - size.X) / 2f, 80f);
            spriteBatch.DrawString(font, title, pos, Color.White, 0f, Vector2.Zero, LabelScale, SpriteEffects.None, 0f);
        }

        /// <summary>Desenha um botão com realce ao hover e texto centrado.</summary>
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

        /// <summary>Desenha o contorno de um retângulo com espessura em pixels.</summary>
        private static void DrawRectOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color color, int thickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}
