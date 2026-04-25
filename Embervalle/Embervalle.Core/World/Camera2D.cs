using Microsoft.Xna.Framework;

namespace Embervalle.Core.World
{
    /// <summary>Câmara 2D simples: deslocamento, zoom e matriz de transformação mundo ↔ ecrã.</summary>
    public sealed class Camera2D
    {
        /// <summary>Origem do mundo visível no canto superior esquerdo (em pixels mundo antes do zoom).</summary>
        public Vector2 Offset { get; set; }
        /// <summary>Fator de escala ecrã ↔ mundo (1 = sem zoom).</summary>
        public float Zoom { get; set; } = 1f;

        /// <summary>Converte coordenadas de ecrã para espaço de mundo.</summary>
        public Vector2 ScreenToWorld(Vector2 screenPosition) => (screenPosition / Zoom) + Offset;

        /// <summary>Converte coordenadas de mundo para espaço de ecrã.</summary>
        public Vector2 WorldToScreen(Vector2 worldPosition) => (worldPosition - Offset) * Zoom;

        /// <summary>Matriz 2D de vista (translação e escala) para o <see cref="SpriteBatch"/>.</summary>
        public Matrix GetTransformMatrix() =>
            Matrix.CreateTranslation(-Offset.X, -Offset.Y, 0f) * Matrix.CreateScale(Zoom, Zoom, 1f);
    }
}
