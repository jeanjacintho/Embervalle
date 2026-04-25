using System;
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>Catálogo de perfis de sprite por biotipo: fornece specs de partes em camadas ou em placeholder único.</summary>
    public static class BodyTypeSpriteCatalog
    {
        
        private const string BodySpriteSheetPath = "Sprites/Characters/body_sprite";

        private static readonly Dictionary<BodyType, BodyTypeSpriteProfile> LayeredProfiles = BuildLayeredProfiles();

        private static readonly Dictionary<BodyType, BodyTypeSpriteProfile> PlaceholderProfiles = BuildPlaceholderProfiles();

        /// <summary>Obtém o perfil de sprite para um biotipo específico.</summary>
        public static BodyTypeSpriteProfile GetLayered(BodyType bodyType) => LayeredProfiles[bodyType];
        ///<summary>Obtém o perfil de sprite para um biotipo específico.</summary>
        public static BodyTypeSpriteProfile GetPlaceholder(BodyType bodyType) => PlaceholderProfiles[bodyType];

        /// <summary>Obtém o caminho para uma parte do sprite para um biotipo específico.</summary>
        public static string PathForPart(BodyType bodyType, CharacterPartSlot slot)
        {
            return (bodyType, slot) switch
            {
                _ => BodySpriteSheetPath,
            };
        }

        /// <summary>Obtém as animações para uma parte do sprite para um biotipo específico.</summary>
        public static IReadOnlyDictionary<CharacterAnimationId, Animation> AnimationsForPart(
            BodyType bodyType,
            CharacterPartSlot slot)
        {
            _ = bodyType;
            return BodySpriteSheetAnimations.GetForPart(slot);
        }
        /// <summary>Obtém o tamanho do frame para uma parte do sprite para um biotipo específico.</summary>
        public static (int FrameWidth, int FrameHeight) FrameSizeForPart(BodyType bodyType, CharacterPartSlot slot)
        {
            _ = bodyType;
            _ = slot;
            return (BodySpriteSheetAnimations.FrameWidth, BodySpriteSheetAnimations.FrameHeight);
        }
        /// <summary>Constrói um dicionário de perfis de sprite para todos os biotipos.</summary>
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
        /// <summary>Constrói um dicionário de partes do sprite para um biotipo específico.</summary>
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
        /// <summary>Cria uma especificação de sprite para uma parte do corpo para um biotipo específico.</summary>
        private static BodyPartSpriteSpec CreatePartSpec(BodyType bodyType, CharacterPartSlot slot)
        {
            string path = PathForPart(bodyType, slot);
            var anims = AnimationsForPart(bodyType, slot);
            (int w, int h) = FrameSizeForPart(bodyType, slot);
            return new BodyPartSpriteSpec(path, w, h, anims);
        }
    }
}
