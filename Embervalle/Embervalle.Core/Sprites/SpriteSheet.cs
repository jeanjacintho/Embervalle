using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Sprites
{
    public sealed class SpriteSheet
    {
        private readonly Texture2D _texture;
        private readonly int _frameWidth;
        private readonly int _frameHeight;
        private readonly int _columns;

        public SpriteSheet(Texture2D texture, int frameWidth, int frameHeight)
        {
            _texture = texture;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _columns = texture.Width / frameWidth;
        }

        public Texture2D Texture => _texture;

        public int FrameWidth => _frameWidth;

        public int FrameHeight => _frameHeight;

        /// <summary>Índice linear → retângulo na textura (grid em linhas, esquerda → direita).</summary>
        public Rectangle GetFrame(int frameIndex)
        {
            int col = frameIndex % _columns;
            int row = frameIndex / _columns;
            return new Rectangle(
                col * _frameWidth,
                row * _frameHeight,
                _frameWidth,
                _frameHeight);
        }

        /// <summary>Linha e coluna explícitas na grelha de frames.</summary>
        public Rectangle GetFrameAtGrid(int row, int col)
        {
            return new Rectangle(
                col * _frameWidth,
                row * _frameHeight,
                _frameWidth,
                _frameHeight);
        }
    }
}
