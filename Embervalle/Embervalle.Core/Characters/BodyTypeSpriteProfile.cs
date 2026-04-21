#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    
    
    public sealed class BodyTypeSpriteProfile
    {
        public BodyTypeSpriteProfile(
            BodyType bodyType,
            IReadOnlyDictionary<CharacterPartSlot, BodyPartSpriteSpec> partsBySlot)
        {
            BodyType = bodyType;
            PartsBySlot = partsBySlot;
        }

        public BodyType BodyType { get; }

        public IReadOnlyDictionary<CharacterPartSlot, BodyPartSpriteSpec> PartsBySlot { get; }

        
        public void ApplyToComposite(CompositeCharacterComponent composite, CharacterAnimationId id)
        {
            bool flipFromTorso = false;
            bool torsoHandledFlip = false;

            for (int i = (int)CharacterPartSlot.Legs; i <= (int)CharacterPartSlot.Head; i++)
            {
                var slot = (CharacterPartSlot)i;
                CharacterPart part = composite.GetPart(slot);
                if (!PartsBySlot.TryGetValue(slot, out BodyPartSpriteSpec? spec))
                {
                    part.PlayingAnimation = null;
                    continue;
                }

                if (!spec.TryResolveAnimation(id, out Animation? template) || template is null)
                {
                    part.PlayingAnimation = null;
                    continue;
                }

                part.PlayingAnimation = template.CloneAndReset();
                if (!torsoHandledFlip && slot == CharacterPartSlot.Torso)
                {
                    flipFromTorso = template.FlipHorizontal;
                    torsoHandledFlip = true;
                }
            }

            if (torsoHandledFlip)
            {
                composite.SpriteFlipHorizontal = flipFromTorso;
            }
            else
            {
                composite.SpriteFlipHorizontal = false;
            }
        }
    }
}
