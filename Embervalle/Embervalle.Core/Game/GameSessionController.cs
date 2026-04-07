using System;

namespace Embervalle.Core.Gameplay
{
    /// <summary>
    /// Mantém o estado da sessão e registra transições no console (útil para debug).
    /// </summary>
    public sealed class GameSessionController
    {
        private GameSessionState _state;

        public GameSessionController(GameSessionState initialState)
        {
            _state = initialState;
            WriteStateLine("início");
        }

        public GameSessionState State => _state;

        public void SetState(GameSessionState newState)
        {
            if (_state == newState)
            {
                return;
            }

            _state = newState;
            WriteStateLine("transição");
        }

        /// <summary>
        /// Alterna entre <see cref="GameSessionState.InGame"/> e <see cref="GameSessionState.Paused"/>.
        /// Em outros estados não altera nada (menu, etc. terão fluxo próprio depois).
        /// </summary>
        public void TogglePause()
        {
            if (_state == GameSessionState.InGame)
            {
                SetState(GameSessionState.Paused);
            }
            else if (_state == GameSessionState.Paused)
            {
                SetState(GameSessionState.InGame);
            }
        }

        private void WriteStateLine(string reason)
        {
            Console.WriteLine($"[Embervalle] Estado da sessão: {_state} ({reason})");
        }
    }
}
