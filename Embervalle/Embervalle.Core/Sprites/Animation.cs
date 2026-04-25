using System;

namespace Embervalle.Core.Sprites
{
    /// <summary>Sequência de índices numa <see cref="SpriteSheet"/>, duração por frame, loop e flip opcional.</summary>
    public sealed class Animation
    {
        public string Name { get; init; } = "";
        public int[] Frames { get; init; } = Array.Empty<int>();
        public float FrameDuration { get; init; }
        public bool Loop { get; init; }
        public bool FlipHorizontal { get; init; }

        private float _timer;
        private int _frameIndex;
        private bool _finished;

        /// <summary>Fábrica de template com nome, frames, duração, loop e flip.</summary>
        public static Animation CreateTemplate(
            string name,
            int[] frames,
            float frameDuration,
            bool loop,
            bool flipHorizontal = false)
        {
            return new Animation
            {
                Name = name,
                Frames = frames,
                FrameDuration = frameDuration,
                Loop = loop,
                FlipHorizontal = flipHorizontal,
            };
        }

        /// <summary>Cópia com contadores de reprodução repostos (nova instância executável).</summary>
        public Animation CloneAndReset()
        {
            return new Animation
            {
                Name = Name,
                Frames = Frames,
                FrameDuration = FrameDuration,
                Loop = Loop,
                FlipHorizontal = FlipHorizontal,
            };
        }

        /// <summary>Índice na folha do frame atual.</summary>
        public int CurrentSheetFrameIndex => Frames.Length == 0 ? 0 : Frames[_frameIndex];

        /// <summary>Verdadeiro quando a animação não faz loop e chegou ao fim.</summary>
        public bool IsFinished => !Loop && _finished;

        /// <summary>Avança o relógio interno e o índice de frame consoante a duração.</summary>
        public void Update(float deltaTime)
        {
            if (Frames.Length == 0 || FrameDuration <= 0f)
            {
                return;
            }

            if (!Loop && _finished)
            {
                return;
            }

            _timer += deltaTime;
            while (_timer >= FrameDuration)
            {
                _timer -= FrameDuration;
                if (_frameIndex < Frames.Length - 1)
                {
                    _frameIndex++;
                }
                else if (Loop)
                {
                    _frameIndex = 0;
                }
                else
                {
                    _finished = true;
                    return;
                }
            }
        }

        /// <summary>Reinicia o temporizador e o índice de frame (mantém definição de frames).</summary>
        public void Reset()
        {
            _timer = 0f;
            _frameIndex = 0;
            _finished = false;
        }
    }
}
