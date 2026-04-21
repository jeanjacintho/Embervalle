using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    
    
    public static class CompositeLayerRenderCoordinator
    {
        public readonly struct CompositeDrawInstance
        {
            public CompositeDrawInstance(CompositeCharacterComponent composite, Vector2 feetWorldPosition, float baseLayerDepth)
            {
                Composite = composite;
                FeetWorldPosition = feetWorldPosition;
                BaseLayerDepth = baseLayerDepth;
            }

            public CompositeCharacterComponent Composite { get; }

            public Vector2 FeetWorldPosition { get; }

            public float BaseLayerDepth { get; }
        }

        
        public static void DrawAllByLayerPass(
            SpriteBatch spriteBatch,
            IReadOnlyList<CompositeDrawInstance> instances,
            CompositeCharacterRenderer renderer)
        {
            for (int slotIndex = 0; slotIndex <= (int)CharacterPartSlot.Head; slotIndex++)
            {
                var slot = (CharacterPartSlot)slotIndex;
                for (int i = 0; i < instances.Count; i++)
                {
                    CompositeDrawInstance inst = instances[i];
                    renderer.DrawSlot(
                        spriteBatch,
                        inst.FeetWorldPosition,
                        inst.Composite,
                        slot,
                        inst.BaseLayerDepth,
                        slotIndex);
                }
            }
        }
    }
}
