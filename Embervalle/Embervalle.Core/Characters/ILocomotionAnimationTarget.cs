namespace Embervalle.Core.Characters
{
    /// <summary>Destino comum para animação de locomoção — composite 4 partes ou sprite único.</summary>
    public interface ILocomotionAnimationTarget
    {
        void SetLogicalAnimation(CharacterAnimationId id);

        void UpdateAnimation(float deltaTime);
    }
}
