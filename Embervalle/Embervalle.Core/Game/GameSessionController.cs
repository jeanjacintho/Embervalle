using System;

namespace Embervalle.Core.Gameplay
{
    /// <summary>Máquina de estados mínima: menu, jogo, pausa; com log de consola para depuração.</summary>
    public sealed class GameSessionController
    {
        private GameSessionState _state;

        /// <summary>Inicializa a máquina de estados e regista o estado inicial na consola.</summary>
        public GameSessionController(GameSessionState initialState)
        {
            _state = initialState;
            WriteStateLine("início");
        }

        /// <summary>Estado atual (menu, jogo, pausa).</summary>
        public GameSessionState State => _state;

        /// <summary>Define o estado se for diferente do atual e regista a transição na consola.</summary>
        public void SetState(GameSessionState newState)
        {
            if (_state == newState)
            {
                return;
            }

            _state = newState;
            WriteStateLine("transição");
        }

        /// <summary>Alterna entre <see cref="GameSessionState.InGame"/> e <see cref="GameSessionState.Paused"/>.</summary>
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

        /// <summary>Escreve linha de depuração com estado e motivo.</summary>
        private void WriteStateLine(string reason) =>
            Console.WriteLine($"[Embervalle] Estado da sessão: {_state} ({reason})");
    }
}
