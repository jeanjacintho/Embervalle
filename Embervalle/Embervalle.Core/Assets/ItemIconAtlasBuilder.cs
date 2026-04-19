using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Assets
{
    /// <summary>
    /// Gera um atlas com celulas 16x16 (doc inventario) — substitua por textura no Content quando quiser.
    /// </summary>
    public static class ItemIconAtlasBuilder
    {
        public const int CellSize = 16;

        public const int Columns = 8;

        public const int Rows = 2;

        public static Texture2D Build(GraphicsDevice device, Texture2D unitPixel, SpriteBatch spriteBatch)
        {
            int w = Columns * CellSize;
            int h = Rows * CellSize;
            var target = new RenderTarget2D(device, w, h);

            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);

            // Placeholder: indices 0+ alinham com iconFrame de itens NAO-arma (armas usam Sprites/Items/weapons.png).
            Color[] colors =
            {
                new Color(120, 120, 130),
                new Color(220, 190, 130),
            };

            int cells = Columns * Rows;
            for (int i = 0; i < cells; i++)
            {
                int col = i % Columns;
                int row = i / Columns;
                Color c = i < colors.Length ? colors[i] : new Color(48, 48, 56);
                spriteBatch.Draw(
                    unitPixel,
                    new Rectangle(col * CellSize, row * CellSize, CellSize, CellSize),
                    c);
            }

            spriteBatch.End();
            device.SetRenderTarget(null);
            return target;
        }
    }
}
