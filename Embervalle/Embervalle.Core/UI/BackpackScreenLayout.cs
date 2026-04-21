using Microsoft.Xna.Framework;

namespace Embervalle.Core.UI
{
    
    public static class BackpackScreenLayout
    {
        public const int GridColumns = 6;

        public const int GridRows = 5;

        public const int CellSize = 44;

        public const int CellGap = 4;

        public static Vector2 GridTopLeft(int viewportWidth, int viewportHeight)
        {
            int gridW = GridColumns * (CellSize + CellGap) - CellGap;
            float x = (viewportWidth - gridW) / 2f;
            float y = 72f;
            return new Vector2(x, y);
        }

        
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

        
        public static Rectangle QuickSlotRect(int viewportHeight, int slotIndex)
        {
            int stride = CellSize + CellGap;
            int x = 12 + slotIndex * stride;
            int y = viewportHeight - stride - 12;
            return new Rectangle(x, y, CellSize, CellSize);
        }

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
