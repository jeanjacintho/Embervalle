#nullable enable
using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>Componente de personagem composto por múltiplas camadas de sprite animadas independentemente.</summary>
    public sealed class CompositeCharacterComponent : ILocomotionAnimationTarget
    {
        public BodyType BodyType { get; set; } = BodyType.Average;

        public BodyTypeSpriteProfile? SpriteProfile { get; set; }

        public bool SpriteFlipHorizontal { get; set; }

        private CharacterAnimationId? _currentLogicalAnimationId;

        public CharacterPart Shadow { get; } = new() { Slot = CharacterPartSlot.Shadow };

        public CharacterPart Legs { get; } = new() { Slot = CharacterPartSlot.Legs };

        public CharacterPart Torso { get; } = new() { Slot = CharacterPartSlot.Torso };

        public CharacterPart Arms { get; } = new() { Slot = CharacterPartSlot.Arms };

        public CharacterPart Head { get; } = new() { Slot = CharacterPartSlot.Head };

        public BodyPartConfig Config => BodyPartConfig.ForStackedBodySprite(BodyType);

        /// <summary>Define a animação lógica para o componente de personagem composto.</summary>
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

        /// <summary>Atualiza a animação do componente de personagem composto.</summary>
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

        /// <summary>Enumera as partes animáveis do componente de personagem composto.</summary>
        private IEnumerable<CharacterPart> EnumerateAnimatableParts()
        {
            yield return Legs;
            yield return Torso;
            yield return Arms;
            yield return Head;
        }

        /// <summary>Obtém uma parte do componente de personagem composto conforme o slot.</summary>
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
