#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Atlas 2048×2048 com regiões nomeadas — um único <see cref="SpriteBatch"/> sem trocar textura (doc 09).
    /// Preencher regiões via TexturePacker JSON ou código.
    /// </summary>
    public sealed class SpriteAtlas
    {
        public SpriteAtlas(Texture2D texture, IReadOnlyDictionary<string, Rectangle> regions)
        {
            Texture = texture;
            Regions = regions;
        }

        public Texture2D Texture { get; }

        public IReadOnlyDictionary<string, Rectangle> Regions { get; }

        public Rectangle GetRegion(string key)
        {
            return Regions[key];
        }

        public bool TryGetRegion(string key, out Rectangle region)
        {
            return Regions.TryGetValue(key, out region);
        }
    }
}
