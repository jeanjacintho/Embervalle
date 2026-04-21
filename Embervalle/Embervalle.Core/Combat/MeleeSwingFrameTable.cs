using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Combat
{
    /// <summary>
    /// Dados de desenho por frame para o swing melee, seguindo o padrão de Stardew Valley
    /// (<c>MeleeWeapon.drawDuringUse</c>): posição relativa aos pés + rotação + flip.
    /// Nossa escala: personagem 48×64 px, arma 32×32 px.
    /// SDV usa player 64×64 (4×) e arma 64×64 (4×); escalamos ~0.75×.
    /// </summary>
    public static class MeleeSwingFrameTable
    {
        public const int FrameCount = 6;

        public readonly struct FrameData
        {
            public readonly Vector2 Offset;
            public readonly float Rotation;
            public readonly SpriteEffects Fx;

            public FrameData(float ox, float oy, float rot, SpriteEffects fx = SpriteEffects.None)
            {
                Offset = new Vector2(ox, oy);
                Rotation = rot;
                Fx = fx;
            }
        }

        // SDV facing codes: 0=Up 1=Right 2=Down 3=Left
        // All offsets relative to feetWorldPosition.
        // SDV playerPos (top-left) → feetPos offset: feet ≈ playerPos + (24, 58)
        // Adapted and scaled to our 48×64 character.

        private static readonly FrameData[] _facingUp =
        {
            new(-8f,  -60f,  -1.963f),   // windup: arma à esquerda apontando cima-direita
            new(  4f,  -68f,  -1.178f),
            new( 18f,  -68f,  -0.393f),
            new( 22f,  -60f,   0.393f),
            new( 22f,  -52f,   1.178f),
            new( 16f,  -44f,   1.963f),  // follow-through: arma à direita
        };

        private static readonly FrameData[] _facingRight =
        {
            new( 14f,  -56f, -0.785f),   // windup: arma em cima
            new( 26f,  -42f,  0.000f),
            new( 32f,  -24f,  0.785f),
            new( 32f,   -8f,  1.571f),
            new( 18f,   -2f,  1.963f),
            new(  2f,   -2f,  2.356f),   // follow-through: arma em baixo
        };

        private static readonly FrameData[] _facingDown =
        {
            new( 28f,  -14f,  0.393f),   // windup: arma à direita, apontando cima
            new( 26f,   -6f,  1.571f),
            new( 20f,    0f,  1.571f),
            new(  6f,    4f,  2.356f),
            new( -4f,    4f,  3.142f),
            new( -8f,    0f,  3.534f),   // follow-through: arma à esquerda, apontando baixo
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

        /// <summary>
        /// Rectangle de dano relativo ao ponto dos pés, por frame e direção.
        /// Similar a <c>getAreaOfEffect</c> do SDV — cresce e varre conforme o swing avança.
        /// </summary>
        public static Rectangle GetHitbox(PlayerCardinalFacing facing, int frame, Vector2 feet)
        {
            FrameData fd = Get(facing, frame);
            // hitbox centrado no ponto da lâmina (perto da ponta da arma, não no cabo)
            // cabo está na origin ≈ (16, 26), ponta a +32 na direção
            Vector2 center = feet + fd.Offset;
            const int hw = 24;
            const int hh = 24;
            return new Rectangle((int)(center.X - hw), (int)(center.Y - hh), hw * 2, hh * 2);
        }
    }
}
