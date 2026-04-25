namespace Embervalle.Core.Characters
{
    /// <summary>Interface para qualquer componente que aceite animações de locomoção definidas por <see cref="CharacterAnimationId"/>.</summary>
    public interface ILocomotionAnimationTarget
    {
        /// <summary>Define a animação lógica atual (idle, walk, ataque) conforme o ID.</summary>
        void SetLogicalAnimation(CharacterAnimationId id);

        /// <summary>Avança frames e estado das animações ativas no delta dado.</summary>
        void UpdateAnimation(float deltaTime);
    }
}
