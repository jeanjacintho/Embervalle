using Embervalle.Core.Sprites;

namespace Embervalle.Core.Assets
{
    
    public static class EmbervalleSheets
    {
        public static SpriteSheet Player { get; private set; } = null!;

        
        public static SpriteSheet ItemIcons { get; private set; } = null!;

        
        public static SpriteSheet WeaponIcons { get; private set; } = null!;

        public static void Load(AssetManager assets)
        {
            Player = assets.LoadSheet("Sprites/Characters/player", 48, 64);
        }

        
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
