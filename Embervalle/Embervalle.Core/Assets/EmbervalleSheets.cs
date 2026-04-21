using Embervalle.Core.Sprites;

namespace Embervalle.Core.Assets
{
    /// <summary>Referências estáticas às sprite sheets após <see cref="Load"/>.</summary>
    public static class EmbervalleSheets
    {
        public static SpriteSheet Player { get; private set; } = null!;

        /// <summary>Atlas de icones de inventario (16x16 por celula) — itens nao-arma (placeholder ou futuro PNG).</summary>
        public static SpriteSheet ItemIcons { get; private set; } = null!;

        /// <summary>Atlas <c>Sprites/Items/weapons.png</c> (32×32 por célula).</summary>
        public static SpriteSheet WeaponIcons { get; private set; } = null!;

        public static void Load(AssetManager assets)
        {
            Player = assets.LoadSheet("Sprites/Characters/player", 48, 64);
        }

        /// <summary>Atlas gerado em runtime ou, no futuro, carregado do Content com o mesmo tamanho de celula.</summary>
        public static void LoadItemIcons(SpriteSheet sheet)
        {
            ItemIcons = sheet;
        }

        public static void LoadWeaponIcons(SpriteSheet sheet)
        {
            WeaponIcons = sheet;
        }
    }
}
