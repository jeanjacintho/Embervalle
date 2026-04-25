using Embervalle.Core.Sprites;

namespace Embervalle.Core.Assets
{
    /// <summary>Referências estáticas às <see cref="SpriteSheet"/> principais (jogador, ícones, armas) após <see cref="Load"/>.</summary>
    public static class EmbervalleSheets
    {
        public static SpriteSheet Player { get; private set; } = null!;
        public static SpriteSheet ItemIcons { get; private set; } = null!;
        public static SpriteSheet WeaponIcons { get; private set; } = null!;

        /// <summary>Carrega a folha de sprites do jogador.</summary>
        public static void Load(AssetManager assets) =>
            Player = assets.LoadSheet("Sprites/Characters/player", 48, 64);

        /// <summary>Carrega o atlas de ícones de itens.</summary>
        public static void LoadItemIcons(SpriteSheet sheet) => ItemIcons = sheet;

        public static void LoadWeaponIcons(SpriteSheet sheet) => WeaponIcons = sheet;
    }
}
