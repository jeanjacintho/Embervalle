#nullable enable
using System;
using Embervalle.Core.Assets;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    /// <summary>Cria instâncias de <see cref="CompositeCharacterComponent"/> a partir de uma <see cref="CharacterDefinition"/>.</summary>
    public static class CompositeCharacterFactory
    {
        /// <summary>Constrói um personagem composto a partir da definição, com placeholder de corpo único ou camadas completas.</summary>
        public static CompositeCharacterComponent FromDefinition(
            CharacterDefinition definition,
            AssetManager assetManager,
            SpriteSheet fallbackSheet,
            bool useSingleFullBodyPlaceholderUntilPartSpritesExist = false)
        {
            if (definition.VisualKind != CharacterVisualKind.CompositeFourPart)
            {
                throw new InvalidOperationException(
                    "Definição não é CompositeFourPart — usa SpriteCharacterFactory.CreateSingleSprite.");
            }

            if (useSingleFullBodyPlaceholderUntilPartSpritesExist)
            {
                return BuildSingleBodyPlaceholder(definition, assetManager, fallbackSheet);
            }

            return BuildFullLayered(definition, assetManager, fallbackSheet);
        }

        /// <summary>Monta um composto com apenas o torso visível a partir de placeholder de folha.</summary>
        private static CompositeCharacterComponent BuildSingleBodyPlaceholder(
            CharacterDefinition definition,
            AssetManager assetManager,
            SpriteSheet fallbackSheet)
        {
            CharacterAppearance a = definition.Appearance;
            BodyTypeSpriteProfile profile = BodyTypeSpriteCatalog.GetPlaceholder(definition.BodyType);

            var composite = new CompositeCharacterComponent
            {
                BodyType = definition.BodyType,
                SpriteProfile = profile,
            };

            if (!profile.PartsBySlot.TryGetValue(CharacterPartSlot.Torso, out BodyPartSpriteSpec? torsoSpec))
            {
                composite.Torso.Sheet = fallbackSheet;
            }
            else
            {
                composite.Torso.Sheet = ResolveSheet(assetManager, torsoSpec, fallbackSheet);
            }

            ApplyCompositeLayerTints(composite, a, torsoOnly: true);
            composite.Torso.IsVisible = true;

            HideNonTorsoPlaceholderLayers(composite);
            composite.Shadow.IsVisible = true;
            composite.SetLogicalAnimation(CharacterAnimationId.IdleDown);
            return composite;
        }

        /// <summary>Monta todas as camadas (pernas, tronco, braços, cabeça) com folhas resolvidas.</summary>
        private static CompositeCharacterComponent BuildFullLayered(
            CharacterDefinition definition,
            AssetManager assetManager,
            SpriteSheet fallbackSheet)
        {
            var composite = new CompositeCharacterComponent
            {
                BodyType = definition.BodyType,
                SpriteProfile = BodyTypeSpriteCatalog.GetLayered(definition.BodyType),
            };

            CharacterAppearance a = definition.Appearance;
            BodyTypeSpriteProfile profile = composite.SpriteProfile!;

            foreach (var kv in profile.PartsBySlot)
            {
                CharacterPartSlot slot = kv.Key;
                BodyPartSpriteSpec spec = kv.Value;
                CharacterPart part = composite.GetPart(slot);
                part.Sheet = ResolveSheet(assetManager, spec, fallbackSheet);
            }

            ApplyCompositeLayerTints(composite, a, torsoOnly: false);

            composite.Legs.IsVisible = true;
            composite.Torso.IsVisible = true;
            composite.Arms.IsVisible = true;
            composite.Head.IsVisible = true;

            composite.Shadow.IsVisible = true;

            composite.SetLogicalAnimation(CharacterAnimationId.IdleDown);
            return composite;
        }

        /// <summary>Aplica cores de aparência às camadas do composto (ou branco se desligado / torso only).</summary>
        private static void ApplyCompositeLayerTints(
            CompositeCharacterComponent composite,
            CharacterAppearance appearance,
            bool torsoOnly)
        {
            if (!appearance.MultiplyCompositeLayersByAppearanceColors)
            {
                composite.Legs.Tint = Color.White;
                composite.Torso.Tint = Color.White;
                composite.Arms.Tint = Color.White;
                composite.Head.Tint = Color.White;
                return;
            }

            if (torsoOnly)
            {
                composite.Torso.Tint = appearance.SkinColor;
                composite.Legs.Tint = Color.White;
                composite.Arms.Tint = Color.White;
                composite.Head.Tint = Color.White;
                return;
            }

            composite.Legs.Tint = appearance.BottomColor;
            composite.Torso.Tint = appearance.TopColor;
            composite.Arms.Tint = appearance.SkinColor;
            composite.Head.Tint = appearance.SkinColor;
        }

        /// <summary>Oculta pernas, braços e cabeça quando só o torso de placeholder é usado.</summary>
        private static void HideNonTorsoPlaceholderLayers(CompositeCharacterComponent c)
        {
            c.Legs.IsVisible = false;
            c.Arms.IsVisible = false;
            c.Head.IsVisible = false;
        }

        /// <summary>Carrega a <see cref="SpriteSheet"/> do spec via <see cref="AssetManager"/>.</summary>
        private static SpriteSheet ResolveSheet(AssetManager assets, BodyPartSpriteSpec spec, SpriteSheet fallback)
        {
            _ = fallback;
            return assets.LoadSheet(spec.ContentPathWithoutExtension, spec.FrameWidth, spec.FrameHeight);
        }
    }
}
