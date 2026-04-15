namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Ponto único para definir qual <see cref="CharacterDefinition"/> usa o jogador.
    /// Atribuir antes de <c>LoadContent</c> (e no futuro após carregar save).
    /// </summary>
    public static class GameCharacterBootstrap
    {
        /// <summary>Definição ativa do herói — por defeito <see cref="CharacterDefinitions.Player"/>.</summary>
        public static CharacterDefinition PlayerDefinition { get; set; } = CharacterDefinitions.Player;
    }
}
