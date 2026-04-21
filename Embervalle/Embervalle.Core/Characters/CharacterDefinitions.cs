using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    
    public static class CharacterDefinitions
    {
        
        public static readonly CharacterDefinition Player = new()
        {
            Id = "player",
            BodyType = BodyType.Average,
            Appearance = new CharacterAppearance
            {
                Body = BodyType.Average,
                EyeShape = "round",
                EyeColor = new Color(50, 70, 110),
                HairStyle = "short",
                HairColor = new Color(55, 35, 22),
                TopStyle = "shirt",
                TopColor = new Color(70, 95, 120),
                BottomStyle = "pants",
                BottomColor = new Color(48, 42, 38),
                Accessory = AccessoryType.None,
                SkinColor = new Color(215, 180, 150),
            },
        };

        public static readonly CharacterDefinition Gareth = new()
        {
            Id = "gareth_blacksmith",
            BodyType = BodyType.Muscular,
            Appearance = new CharacterAppearance
            {
                Body = BodyType.Muscular,
                EyeShape = "small",
                EyeColor = new Color(60, 40, 20),
                HairStyle = "short",
                HairColor = new Color(25, 15, 10),
                TopStyle = "apron",
                TopColor = new Color(90, 60, 40),
                BottomStyle = "pants",
                BottomColor = new Color(45, 45, 55),
                Accessory = AccessoryType.None,
                SkinColor = new Color(180, 130, 90),
            },
        };

        public static readonly CharacterDefinition Maya = new()
        {
            Id = "maya_farmer",
            BodyType = BodyType.Slim,
            Appearance = new CharacterAppearance
            {
                Body = BodyType.Slim,
                EyeShape = "round",
                EyeColor = new Color(40, 80, 120),
                HairStyle = "long",
                HairColor = new Color(140, 60, 30),
                TopStyle = "shirt",
                TopColor = new Color(50, 120, 70),
                BottomStyle = "pants",
                BottomColor = new Color(55, 45, 35),
                Accessory = AccessoryType.HatStraw,
                SkinColor = new Color(235, 195, 170),
            },
        };

        public static readonly CharacterDefinition Timo = new()
        {
            Id = "timo_child",
            BodyType = BodyType.Short,
            Appearance = new CharacterAppearance
            {
                Body = BodyType.Short,
                HairStyle = "short",
                HairColor = new Color(220, 190, 80),
                TopColor = new Color(200, 60, 50),
                SkinColor = new Color(245, 210, 180),
            },
        };

        public static readonly CharacterDefinition Bron = new()
        {
            Id = "bron_innkeeper",
            BodyType = BodyType.Fat,
            Appearance = new CharacterAppearance
            {
                Body = BodyType.Fat,
                HairStyle = "short",
                HairColor = new Color(35, 30, 28),
                TopStyle = "apron",
                TopColor = new Color(240, 240, 235),
                SkinColor = new Color(160, 120, 90),
            },
        };

        public static readonly CharacterDefinition Sera = new()
        {
            Id = "sera_merchant",
            BodyType = BodyType.Average,
            Appearance = new CharacterAppearance
            {
                TopStyle = "jacket",
                TopColor = new Color(40, 70, 140),
                Accessory = AccessoryType.Glasses,
                SkinColor = new Color(230, 200, 175),
                HairColor = new Color(90, 55, 35),
            },
        };
    }
}
