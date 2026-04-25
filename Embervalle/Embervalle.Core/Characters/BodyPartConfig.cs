using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Configuração de offsets e escalas das partes do sprite de um personagem composto conforme o biotipo.</summary>
    public sealed class BodyPartConfig
    {
        public Vector2 LegsOffset;
        public Vector2 TorsoOffset;
        public Vector2 ArmsOffset;
        public Vector2 HeadOffset;

        public Vector2 LegsScale = Vector2.One;
        public Vector2 TorsoScale = Vector2.One;
        public Vector2 ArmsScale = Vector2.One;
        public Vector2 HeadScale = Vector2.One;

        /// <summary>Obtém o offset de uma parte do sprite conforme o slot.</summary>
        public Vector2 GetOffset(CharacterPartSlot slot)
        {
            return slot switch
            {
                CharacterPartSlot.Shadow => LegsOffset,
                CharacterPartSlot.Legs => LegsOffset,
                CharacterPartSlot.Torso => TorsoOffset,
                CharacterPartSlot.Arms => ArmsOffset,
                CharacterPartSlot.Head => HeadOffset,
                _ => Vector2.Zero,
            };
        }

        /// <summary>Obtém a escala de uma parte do sprite conforme o slot.</summary>
        public Vector2 GetScale(CharacterPartSlot slot)
        {
            return slot switch
            {
                CharacterPartSlot.Legs => LegsScale,
                CharacterPartSlot.Torso => TorsoScale,
                CharacterPartSlot.Arms => ArmsScale,
                CharacterPartSlot.Head => HeadScale,
                _ => Vector2.One,
            };
        }

        /// <summary>Cria uma configuração de partes do sprite para um biotipo específico, ajustando offsets para stack de sprites.</summary>
        public static BodyPartConfig ForStackedBodySprite(BodyType bodyType)
        {
            BodyPartConfig c = For(bodyType);
            c.LegsOffset = Vector2.Zero;
            c.TorsoOffset = Vector2.Zero;
            c.ArmsOffset = Vector2.Zero;
            c.HeadOffset = Vector2.Zero;
            return c;
        }

        /// <summary>Cria uma configuração de partes do sprite para um biotipo específico, ajustando offsets para stack de sprites.</summary>
        public static BodyPartConfig For(BodyType body)
        {
            return body switch
            {
                BodyType.Average => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -8),
                    TorsoOffset = new Vector2(0, -18),
                    ArmsOffset = new Vector2(0, -16),
                    HeadOffset = new Vector2(0, -36),
                    TorsoScale = new Vector2(1f, 1f),
                    HeadScale = new Vector2(1f, 1f),
                },
                BodyType.Muscular => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -8),
                    TorsoOffset = new Vector2(0, -20),
                    ArmsOffset = new Vector2(0, -17),
                    HeadOffset = new Vector2(0, -40),
                    TorsoScale = new Vector2(1.3f, 1f),
                    HeadScale = new Vector2(1f, 1f),
                },
                BodyType.Slim => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -8),
                    TorsoOffset = new Vector2(0, -17),
                    ArmsOffset = new Vector2(0, -15),
                    HeadOffset = new Vector2(0, -33),
                    TorsoScale = new Vector2(0.85f, 1f),
                    HeadScale = new Vector2(1f, 1f),
                },
                BodyType.Short => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -6),
                    TorsoOffset = new Vector2(0, -12),
                    ArmsOffset = new Vector2(0, -12),
                    HeadOffset = new Vector2(0, -24),
                    LegsScale = new Vector2(1f, 0.75f),
                    TorsoScale = new Vector2(0.9f, 0.9f),
                    HeadScale = new Vector2(1.1f, 1.1f),
                },
                BodyType.Fat => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -8),
                    TorsoOffset = new Vector2(0, -16),
                    ArmsOffset = new Vector2(0, -16),
                    HeadOffset = new Vector2(0, -32),
                    TorsoScale = new Vector2(1.4f, 1f),
                    HeadScale = new Vector2(1.1f, 1.1f),
                },
                BodyType.Tall => new BodyPartConfig
                {
                    LegsOffset = new Vector2(0, -10),
                    TorsoOffset = new Vector2(0, -22),
                    ArmsOffset = new Vector2(0, -18),
                    HeadOffset = new Vector2(0, -44),
                    TorsoScale = new Vector2(1f, 1f),
                    HeadScale = new Vector2(0.95f, 0.95f),
                },
                _ => For(BodyType.Average),
            };
        }
    }
}
