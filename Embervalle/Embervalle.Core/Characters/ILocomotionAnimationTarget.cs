namespace Embervalle.Core.Characters
{
    
    public interface ILocomotionAnimationTarget
    {
        void SetLogicalAnimation(CharacterAnimationId id);

        void UpdateAnimation(float deltaTime);
    }
}
