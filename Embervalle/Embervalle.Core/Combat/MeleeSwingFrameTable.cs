using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Combat
{
    /// <summary>Tabela de dados por frame da animação de golpe corpo a corpo: offset, rotação e hitbox para cada direção cardinal.</summary>
    public static class MeleeSwingFrameTable
    {
        public const int FrameCount = 6;

        /// <summary>Dados de posicionamento e rotação do sprite de arma para um único frame do golpe.</summary>
        public readonly struct FrameData
        {
            public readonly Vector2 Offset;
            public readonly float Rotation;
            public readonly SpriteEffects Fx;

            /// <summary>Constrói dados de frame com offset, rotação (radianos) e flip opcional.</summary>
            public FrameData(float ox, float oy, float rot, SpriteEffects fx = SpriteEffects.None)
            {
                Offset = new Vector2(ox, oy);
                Rotation = rot;
                Fx = fx;
            }
        }

        private static readonly FrameData[] _facingUp =
        {
            new(-8f,  -60f,  -1.963f),   
            new(  4f,  -68f,  -1.178f),
            new( 18f,  -68f,  -0.393f),
            new( 22f,  -60f,   0.393f),
            new( 22f,  -52f,   1.178f),
            new( 16f,  -44f,   1.963f),  
        };

        private static readonly FrameData[] _facingRight =
        {
            new( 14f,  -56f, -0.785f),   
            new( 26f,  -42f,  0.000f),
            new( 32f,  -24f,  0.785f),
            new( 32f,   -8f,  1.571f),
            new( 18f,   -2f,  1.963f),
            new(  2f,   -2f,  2.356f),   
        };

        private static readonly FrameData[] _facingDown =
        {
            new( 28f,  -14f,  0.393f),   
            new( 26f,   -6f,  1.571f),
            new( 20f,    0f,  1.571f),
            new(  6f,    4f,  2.356f),
            new( -4f,    4f,  3.142f),
            new( -8f,    0f,  3.534f),   
        };

        private static readonly FrameData[] _facingLeft =
        {
            new(-10f,  -56f,  0.785f, SpriteEffects.FlipHorizontally),
            new(-26f,  -42f,  0.000f, SpriteEffects.FlipHorizontally),
            new(-32f,  -24f, -0.785f, SpriteEffects.FlipHorizontally),
            new(-32f,   -8f, -1.571f, SpriteEffects.FlipHorizontally),
            new(-18f,   -2f, -1.963f, SpriteEffects.FlipHorizontally),
            new( -2f,   -2f, -2.356f, SpriteEffects.FlipHorizontally),
        };

        /// <summary>Obtém offset, rotação e <see cref="SpriteEffects"/> do frame de golpe para a direção cardinal.</summary>
        public static FrameData Get(PlayerCardinalFacing facing, int frame)
        {
            FrameData[] table = facing switch
            {
                PlayerCardinalFacing.Up => _facingUp,
                PlayerCardinalFacing.Right => _facingRight,
                PlayerCardinalFacing.Down => _facingDown,
                PlayerCardinalFacing.Left => _facingLeft,
                _ => _facingDown,
            };
            int f = frame < 0 ? 0 : frame >= FrameCount ? FrameCount - 1 : frame;
            return table[f];
        }

        /// <summary>Retângulo aproximado de hitbox de melée em torno do offset do frame.</summary>
        public static Rectangle GetHitbox(PlayerCardinalFacing facing, int frame, Vector2 feet)
        {
            FrameData fd = Get(facing, frame);
            Vector2 center = feet + fd.Offset;
            const int hw = 24;
            const int hh = 24;
            return new Rectangle((int)(center.X - hw), (int)(center.Y - hh), hw * 2, hh * 2);
        }
    }
}
