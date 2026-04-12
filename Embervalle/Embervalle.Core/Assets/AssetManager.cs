#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Assets
{
    /// <summary>
    /// Único ponto de carregamento de texturas / sheets (cache por caminho lógico do Content).
    /// </summary>
    public sealed class AssetManager
    {
        private readonly ContentManager _content;
        private readonly Dictionary<string, SpriteSheet> _sheets = new();

        public AssetManager(ContentManager content)
        {
            _content = content;
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
