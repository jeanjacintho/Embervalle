using Microsoft.Xna.Framework;

namespace Embervalle.Core.UI
{
    /// <summary>Calcula layout, hit-test e retângulos de células da grade de inventário e dos slots rápidos da mochila.</summary>
    public static class BackpackScreenLayout
    {
        public const int GridColumns = 6;

        public const int GridRows = 5;

        public const int CellSize = 44;

        public const int CellGap = 4;

        /// <summary>Canto superior esquerdo da grelha de inventário, centrado na horizontal.</summary>
        public static Vector2 GridTopLeft(int viewportWidth, int viewportHeight)
        {
            int gridW = GridColumns * (CellSize + CellGap) - CellGap;
            float x = (viewportWidth - gridW) / 2f;
            float y = 72f;
            return new Vector2(x, y);
        }

        /// <summary>Se o ponto estiver numa célula da grelha, devolve o índice linear; senão null.</summary>
        public static int? HitTestGrid(int viewportWidth, int viewportHeight, Point mouse)
        {
            Vector2 origin = GridTopLeft(viewportWidth, viewportHeight);
            float mx = mouse.X - origin.X;
            float my = mouse.Y - origin.Y;
            if (mx < 0 || my < 0)
            {
                return null;
            }

            int stride = CellSize + CellGap;
            int col = (int)(mx / stride);
            int row = (int)(my / stride);
            if (col < 0 || col >= GridColumns || row < 0 || row >= GridRows)
            {
                return null;
            }

            float lx = mx - col * stride;
            float ly = my - row * stride;
            if (lx > CellSize || ly > CellSize)
            {
                return null;
            }

            return row * GridColumns + col;
        }

        /// <summary>Retângulo em ecrã da célula de inventário pelo índice (row-major).</summary>
        public static Rectangle GridCellRect(int viewportWidth, int viewportHeight, int slotIndex)
        {
            Vector2 origin = GridTopLeft(viewportWidth, viewportHeight);
            int col = slotIndex % GridColumns;
            int row = slotIndex / GridColumns;
            int stride = CellSize + CellGap;
            return new Rectangle(
                (int)(origin.X + col * stride),
                (int)(origin.Y + row * stride),
                CellSize,
                CellSize);
        }

        /// <summary>Retângulo do slot rápido (toolbar) na borda inferior esquerda.</summary>
        public static Rectangle QuickSlotRect(int viewportHeight, int slotIndex)
        {
            int stride = CellSize + CellGap;
            int x = 12 + slotIndex * stride;
            int y = viewportHeight - stride - 12;
            return new Rectangle(x, y, CellSize, CellSize);
        }

        /// <summary>Se o ponto estiver num slot rápido (0 ou 1), devolve o índice; senão null.</summary>
        public static int? HitTestQuickSlot(int viewportHeight, Point mouse)
        {
            for (int i = 0; i < 2; i++)
            {
                if (QuickSlotRect(viewportHeight, i).Contains(mouse))
                {
                    return i;
                }
            }

            return null;
        }
    }
}
