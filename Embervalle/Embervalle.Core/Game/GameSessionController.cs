using System;

namespace Embervalle.Core.Gameplay
{
    
    
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
