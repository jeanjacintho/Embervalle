#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>Humanoide em 4 partes + sombra — cada parte tem a sua instância de animação.</summary>
    public sealed class CompositeCharacterComponent : ILocomotionAnimationTarget
    {
        public BodyType BodyType { get; set; } = BodyType.Average;

        /// <summary>Definido na factory — usado por <see cref="SetLogicalAnimation"/>.</summary>
        public BodyTypeSpriteProfile? SpriteProfile { get; set; }

        /// <summary>Espelhar todas as camadas (ex.: walk left com arte só para a direita).</summary>
        public bool SpriteFlipHorizontal { get; set; }

        private CharacterAnimationId? _currentLogicalAnimationId;

        public CharacterPart Shadow { get; } = new() { Slot = CharacterPartSlot.Shadow };

        public CharacterPart Legs { get; } = new() { Slot = CharacterPartSlot.Legs };

        public CharacterPart Torso { get; } = new() { Slot = CharacterPartSlot.Torso };

        public CharacterPart Arms { get; } = new() { Slot = CharacterPartSlot.Arms };

        public CharacterPart Head { get; } = new() { Slot = CharacterPartSlot.Head };

        public BodyPartConfig Config => BodyPartConfig.ForStackedBodySprite(BodyType);

        public void SetLogicalAnimation(CharacterAnimationId id)
        {
            if (SpriteProfile == null)
            {
                return;
            }

            if (_currentLogicalAnimationId == id)
            {
                return;
            }

            _currentLogicalAnimationId = id;
            SpriteProfile.ApplyToComposite(this, id);
        }

        public void UpdateAnimation(float deltaTime)
        {
            foreach (CharacterPart part in EnumerateAnimatableParts())
            {
                if (!part.IsVisible)
                {
                    continue;
                }

                part.PlayingAnimation?.Update(deltaTime);
            }
        }

        private IEnumerable<CharacterPart> EnumerateAnimatableParts()
        {
            yield return Legs;
            yield return Torso;
            yield return Arms;
            yield return Head;
        }

        public CharacterPart GetPart(CharacterPartSlot slot)
        {
            return slot switch
            {
                CharacterPartSlot.Shadow => Shadow,
                CharacterPartSlot.Legs => Legs,
                CharacterPartSlot.Torso => Torso,
                CharacterPartSlot.Arms => Arms,
                CharacterPartSlot.Head => Head,
                _ => Shadow,
            };
        }
    }
}
