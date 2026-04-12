using Embervalle.Core.Sprites;

namespace Embervalle.Core.Assets
{
    /// <summary>Referências estáticas às sprite sheets após <see cref="Load"/>.</summary>
    public static class EmbervalleSheets
    {
        public static SpriteSheet Player { get; private set; } = null!;

        public static void Load(AssetManager assets)
        {
            Player = assets.LoadSheet("Sprites/Characters/player", 48, 64);
        }
    }
}
