using System;

namespace Embervalle.Core.Sprites
{
    
    
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

        public int CurrentSheetFrameIndex => Frames.Length == 0 ? 0 : Frames[_frameIndex];

        public bool IsFinished => !Loop && _finished;

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

        public void Reset()
        {
            _timer = 0f;
            _frameIndex = 0;
            _finished = false;
        }
    }
}
