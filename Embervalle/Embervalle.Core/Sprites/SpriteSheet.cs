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
