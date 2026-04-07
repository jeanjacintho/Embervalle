namespace Embervalle.Core.Gameplay
{
    /// <summary>
    /// Estado de alto nível da sessão de jogo (menu, jogando, pausado).
    /// Expanda aqui quando houver fluxo de splash, loading, etc.
    /// </summary>
    public enum GameSessionState
    {
        MainMenu,
        InGame,
        Paused,
    }
}
