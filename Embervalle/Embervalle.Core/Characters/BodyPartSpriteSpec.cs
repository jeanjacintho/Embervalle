#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    
    
    public sealed class BodyPartSpriteSpec
    {
        public BodyPartSpriteSpec(
            string contentPathWithoutExtension,
            int frameWidth,
            int frameHeight,
            IReadOnlyDictionary<CharacterAnimationId, Animation> animationsById)
        {
            ContentPathWithoutExtension = contentPathWithoutExtension;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            AnimationsById = animationsById;
        }

        public string ContentPathWithoutExtension { get; }

        public int FrameWidth { get; }

        public int FrameHeight { get; }

        public IReadOnlyDictionary<CharacterAnimationId, Animation> AnimationsById { get; }

        public bool TryResolveAnimation(CharacterAnimationId id, out Animation? template)
        {
            if (AnimationsById.TryGetValue(id, out template))
            {
                return true;
            }

            if (AnimationsById.TryGetValue(CharacterAnimationId.IdleDown, out template))
            {
                return true;
            }

            template = null;
            return false;
        }
    }
}
