using System;
using System.Collections.Generic;

namespace Embervalle.Core.Combat
{
    /// <summary>Componente de recarga de habilidades: rastreia timers por ID e verifica se uma ação ainda está em cooldown.</summary>
    public sealed class CooldownComponent
    {
        private readonly Dictionary<string, float> _timers = new();

        /// <summary>Indica se o identificador de habilidade ainda tem tempo de recarga.</summary>
        public bool IsOnCooldown(string id) =>
            _timers.TryGetValue(id, out float t) && t > 0f;

        /// <summary>Define a duração restante de recarga para o id dado (substitui se já existir).</summary>
        public void StartCooldown(string id, float duration) => _timers[id] = duration;

        /// <summary>Decrementa temporizadores e remove entradas que chegaram a zero.</summary>
        public void Update(float deltaSeconds)
        {
            if (_timers.Count == 0)
            {
                return;
            }

            List<string>? keys = null;
            foreach (KeyValuePair<string, float> kv in _timers)
            {
                float next = Math.Max(0f, kv.Value - deltaSeconds);
                _timers[kv.Key] = next;
                if (next <= 0f)
                {
                    keys ??= new List<string>();
                    keys.Add(kv.Key);
                }
            }

            if (keys != null)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    _timers.Remove(keys[i]);
                }
            }
        }
    }
}
