using Embervalle.Core.Characters;

namespace Embervalle.Core.Sprites
{
    /// <summary>Encaminha <see cref="ILocomotionAnimationTarget"/> para um único <see cref="SpriteComponent"/> (personagem 1 ficha).</summary>
    public sealed class SingleSpriteLocomotionAdapter : ILocomotionAnimationTarget
    {
        private readonly SpriteComponent _sprite;

        public SingleSpriteLocomotionAdapter(SpriteComponent sprite) => _sprite = sprite;

        public void SetLogicalAnimation(CharacterAnimationId id) =>
            _sprite.SetAnimation(CharacterAnimationLibrary.GetTemplate(id));

        public void UpdateAnimation(float deltaTime) => _sprite.Update(deltaTime);
    }
}
