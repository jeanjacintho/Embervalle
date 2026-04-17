using System;

namespace Embervalle.Core.Combat
{
    public sealed class ManaComponent
    {
        public float Current { get; set; }

        public float Max { get; set; }

        public float RegenPerSecond { get; set; }

        public void Update(float deltaSeconds)
        {
            if (Current < Max)
            {
                Current = Math.Min(Max, Current + RegenPerSecond * deltaSeconds);
            }
        }
    }
}
