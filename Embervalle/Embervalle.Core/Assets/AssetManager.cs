#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Assets
{
    
    
    public sealed class AssetManager
    {
        private readonly ContentManager _content;
        private readonly Dictionary<string, SpriteSheet> _sheets = new();
        private readonly Dictionary<string, Texture2D> _textures = new();

        public AssetManager(ContentManager content)
        {
            _content = content;
        }

        
        public Texture2D? TryLoadTexture(string contentPathWithoutExtension)
        {
            if (string.IsNullOrWhiteSpace(contentPathWithoutExtension))
            {
                return null;
            }

            if (_textures.TryGetValue(contentPathWithoutExtension, out Texture2D? cached))
            {
                return cached;
            }

            try
            {
                Texture2D texture = _content.Load<Texture2D>(contentPathWithoutExtension);
                _textures[contentPathWithoutExtension] = texture;
                return texture;
            }
            catch (ContentLoadException)
            {
                return null;
            }
        }

        public SpriteSheet LoadSheet(string contentPathWithoutExtension, int frameWidth, int frameHeight)
        {
            if (_sheets.TryGetValue(contentPathWithoutExtension, out SpriteSheet? cached))
            {
                return cached;
            }

            Texture2D texture = _content.Load<Texture2D>(contentPathWithoutExtension);
            var sheet = new SpriteSheet(texture, frameWidth, frameHeight);
            _sheets[contentPathWithoutExtension] = sheet;
            return sheet;
        }
    }
}
