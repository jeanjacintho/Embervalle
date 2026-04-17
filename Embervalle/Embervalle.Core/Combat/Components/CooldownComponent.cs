using System;
using System.Collections.Generic;

namespace Embervalle.Core.Combat
{
    public sealed class CooldownComponent
    {
        private readonly Dictionary<string, float> _timers = new();

        public bool IsOnCooldown(string id) =>
            _timers.TryGetValue(id, out float t) && t > 0f;

        public void StartCooldown(string id, float duration) => _timers[id] = duration;

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
