using System;
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Ponto central: para cada <see cref="BodyType"/> define PNG e animações por parte (pernas, torso, braços, cabeça).
    /// Ajusta <see cref="PathForPart"/> e <see cref="AnimationsForPart"/>.
    /// </summary>
    public static class BodyTypeSpriteCatalog
    {
        /// <summary>Ver <see cref="BodySpriteSheetAnimations"/> — mesma textura para as 4 partes.</summary>
        private const string BodySpriteSheetPath = "Sprites/Characters/body_sprite";

        private static readonly Dictionary<BodyType, BodyTypeSpriteProfile> LayeredProfiles = BuildLayeredProfiles();

        private static readonly Dictionary<BodyType, BodyTypeSpriteProfile> PlaceholderProfiles = BuildPlaceholderProfiles();

        /// <summary>Quatro partes empilhadas — uma sheet por parte (ou a mesma sheet repetida até haver arte).</summary>
        public static BodyTypeSpriteProfile GetLayered(BodyType bodyType) => LayeredProfiles[bodyType];

        /// <summary>Só torso + sombra (placeholder até haver arte separada por parte).</summary>
        public static BodyTypeSpriteProfile GetPlaceholder(BodyType bodyType) => PlaceholderProfiles[bodyType];

        /// <summary>Caminho no Content (sem extensão) por tipo de corpo e parte.</summary>
        public static string PathForPart(BodyType bodyType, CharacterPartSlot slot)
        {
            return (bodyType, slot) switch
            {
                // Ex.: (BodyType.Tall, CharacterPartSlot.Arms) => "Sprites/Characters/body_tall/arms",
                // (BodyType.Tall, CharacterPartSlot.Legs) => "Sprites/Characters/body_tall/legs",
                // (BodyType.Tall, CharacterPartSlot.Torso) => "Sprites/Characters/body_tall/torso",
                // (BodyType.Tall, CharacterPartSlot.Head) => "Sprites/Characters/body_tall/head",
                _ => BodySpriteSheetPath,
            };
        }

        public static IReadOnlyDictionary<CharacterAnimationId, Animation> AnimationsForPart(
            BodyType bodyType,
            CharacterPartSlot slot)
        {
            _ = bodyType;
            return BodySpriteSheetAnimations.GetForPart(slot);
        }

        public static (int FrameWidth, int FrameHeight) FrameSizeForPart(BodyType bodyType, CharacterPartSlot slot)
        {
            _ = bodyType;
            _ = slot;
            return (BodySpriteSheetAnimations.FrameWidth, BodySpriteSheetAnimations.FrameHeight);
        }

        private static Dictionary<BodyType, BodyTypeSpriteProfile> BuildLayeredProfiles()
        {
            var d = new Dictionary<BodyType, BodyTypeSpriteProfile>();
            foreach (BodyType bt in Enum.GetValues<BodyType>())
            {
                d[bt] = new BodyTypeSpriteProfile(bt, BuildLayeredPartsFor(bt));
            }

            return d;
        }

        private static Dictionary<BodyType, BodyTypeSpriteProfile> BuildPlaceholderProfiles()
        {
            var d = new Dictionary<BodyType, BodyTypeSpriteProfile>();
            foreach (BodyType bt in Enum.GetValues<BodyType>())
            {
                d[bt] = new BodyTypeSpriteProfile(
                    bt,
                    new Dictionary<CharacterPartSlot, BodyPartSpriteSpec>
                    {
                        [CharacterPartSlot.Torso] = CreatePartSpec(bt, CharacterPartSlot.Torso),
                    });
            }

            return d;
        }

        private static IReadOnlyDictionary<CharacterPartSlot, BodyPartSpriteSpec> BuildLayeredPartsFor(BodyType bodyType)
        {
            return new Dictionary<CharacterPartSlot, BodyPartSpriteSpec>
            {
                [CharacterPartSlot.Legs] = CreatePartSpec(bodyType, CharacterPartSlot.Legs),
                [CharacterPartSlot.Torso] = CreatePartSpec(bodyType, CharacterPartSlot.Torso),
                [CharacterPartSlot.Arms] = CreatePartSpec(bodyType, CharacterPartSlot.Arms),
                [CharacterPartSlot.Head] = CreatePartSpec(bodyType, CharacterPartSlot.Head),
            };
        }

        private static BodyPartSpriteSpec CreatePartSpec(BodyType bodyType, CharacterPartSlot slot)
        {
            string path = PathForPart(bodyType, slot);
            var anims = AnimationsForPart(bodyType, slot);
            (int w, int h) = FrameSizeForPart(bodyType, slot);
            return new BodyPartSpriteSpec(path, w, h, anims);
        }
    }
}
