#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    /// <summary>Atlas de sprites que agrupa uma textura e um dicionário de regiões nomeadas para lookup eficiente.</summary>
    public sealed class SpriteAtlas
    {
        /// <summary>Associa a textura ao mapa de regiões nomeadas (UV rects).</summary>
        public SpriteAtlas(Texture2D texture, IReadOnlyDictionary<string, Rectangle> regions)
        {
            Texture = texture;
            Regions = regions;
        }

        public Texture2D Texture { get; }

        public IReadOnlyDictionary<string, Rectangle> Regions { get; }

        /// <summary>Obtém a região pelo nome; lança se a chave não existir.</summary>
        public Rectangle GetRegion(string key)
        {
            return Regions[key];
        }

        /// <summary>Tenta obter a região; devolve false se a chave não existir.</summary>
        public bool TryGetRegion(string key, out Rectangle region)
        {
            return Regions.TryGetValue(key, out region);
        }
    }
}
