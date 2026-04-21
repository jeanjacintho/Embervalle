#nullable enable
using System;
using Embervalle.Core.Assets;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Characters
{
    
    public static class CompositeCharacterFactory
    {
        
        
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

        private static void HideNonTorsoPlaceholderLayers(CompositeCharacterComponent c)
        {
            c.Legs.IsVisible = false;
            c.Arms.IsVisible = false;
            c.Head.IsVisible = false;
        }

        private static SpriteSheet ResolveSheet(AssetManager assets, BodyPartSpriteSpec spec, SpriteSheet fallback)
        {
            _ = fallback;
            return assets.LoadSheet(spec.ContentPathWithoutExtension, spec.FrameWidth, spec.FrameHeight);
        }
    }
}
