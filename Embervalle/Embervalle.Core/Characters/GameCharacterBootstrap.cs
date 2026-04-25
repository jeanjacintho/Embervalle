namespace Embervalle.Core.Characters
{
    /// <summary>Ponto de entrada para configurar a definição do personagem jogador antes de iniciar a sessão de jogo.</summary>
    public static class GameCharacterBootstrap
    {
        /// <summary>Definição ativa do jogador (pode ser trocada antes de criar a sessão).</summary>
        public static CharacterDefinition PlayerDefinition { get; set; } = CharacterDefinitions.Player;
    }
}
