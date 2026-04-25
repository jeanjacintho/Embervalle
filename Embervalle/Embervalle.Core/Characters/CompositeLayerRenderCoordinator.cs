using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Embervalle.Core.Characters
{
    /// <summary>Coordena a renderização de múltiplos personagens compostos por passagem de camada, garantindo ordem de depth correta.</summary>
    public static class CompositeLayerRenderCoordinator
    {
        /// <summary>Dados de uma instância de personagem composto para submissão ao renderizador de camadas.</summary>
        public readonly struct CompositeDrawInstance
        {
            /// <summary>Encapsula composto, posição dos pés e depth base para desenho.</summary>
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

        /// <summary>Desenha todas as instâncias por passagem de slot: primeiro todas as pernas, depois tronco, etc., para intercalar corretamente entre inimigos.</summary>
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
