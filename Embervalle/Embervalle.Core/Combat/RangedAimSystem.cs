using Embervalle.Core.Input;
using Embervalle.Core.Sprites;
using Embervalle.Core.World;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    
    
    public sealed class RangedAimSystem
    {
        private const float MinAimLengthSq = 0.01f;

        public Vector2 AimDirection { get; private set; } = Vector2.UnitX;

        public Vector2 AimWorldPosition { get; private set; }

        public void Update(Vector2 playerWorldPos, Camera2D camera, InputManager input)
        {
            Vector2 mouseWorld = camera.ScreenToWorld(input.MousePosition);
            Vector2 rawAim = mouseWorld - playerWorldPos;
            if (rawAim.LengthSquared() > MinAimLengthSq)
            {
                AimDirection = Vector2.Normalize(rawAim);
            }

            AimWorldPosition = playerWorldPos + AimDirection * 64f;
        }
    }
}
