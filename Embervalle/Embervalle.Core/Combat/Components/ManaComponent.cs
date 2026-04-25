using System;

namespace Embervalle.Core.Combat
{
    /// <summary>Componente de mana de um personagem com regeneração automática ao longo do tempo.</summary>
    public sealed class ManaComponent
    {
        public float Current { get; set; }

        public float Max { get; set; }

        public float RegenPerSecond { get; set; }

        /// <summary>Regenera mana até ao máximo conforme o delta.</summary>
        public void Update(float deltaSeconds)
        {
            if (Current < Max)
            {
                Current = Math.Min(Max, Current + RegenPerSecond * deltaSeconds);
            }
        }
    }
}
