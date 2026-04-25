using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Aparência visual de um personagem: cores, estilos de roupa, cabelo e acessórios.</summary>
    public sealed class CharacterAppearance
    {
        public BodyType Body { get; set; } = BodyType.Average;

        public string EyeShape { get; set; } = "round";

        public Color EyeColor { get; set; } = Color.Blue;

        public string HairStyle { get; set; } = "short";

        public Color HairColor { get; set; } = new Color(80, 40, 20);

        public string TopStyle { get; set; } = "shirt";

        public Color TopColor { get; set; } = Color.Red;

        public string BottomStyle { get; set; } = "pants";

        public Color BottomColor { get; set; } = Color.DarkBlue;

        public AccessoryType Accessory { get; set; } = AccessoryType.None;

        public Color SkinColor { get; set; } = new Color(220, 175, 130);

        public bool MultiplyCompositeLayersByAppearanceColors { get; set; }
    }
}
