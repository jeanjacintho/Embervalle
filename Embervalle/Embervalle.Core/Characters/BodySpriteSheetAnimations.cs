using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>
    /// Keyframes para <c>Sprites/Characters/body_sprite</c>: grelha com células <b>48×64 px</b> (24 colunas; idle linhas 0–3, walk 4–7).
    /// Linhas 1–4 (índice 0–3): idle — cabeça, torso, braços, pernas.
    /// Linhas 5–8 (índice 4–7): walk — mesma ordem.
    /// Colunas 1–5 (índice 0–4): frente; 6–10 (5–9): costas; 11–15 (10–14): esquerda; 16–20 (15–19): direita — idle com 5 frames/dir.
    /// Walk: 6 frames/dir — colunas 0–5, 6–11, 12–17, 18–23.
    /// Índice linear = <see cref="SheetColumns"/> × linha + coluna.
    /// </summary>
    public static class BodySpriteSheetAnimations
    {
        public const int SheetColumns = 24;

        public const int FrameWidth = 48;

        public const int FrameHeight = 64;

        private const float IdleFrameDuration = 0.32f;

        private const float WalkFrameDuration = 0.10f;

        private static readonly IReadOnlyDictionary<CharacterPartSlot, IReadOnlyDictionary<CharacterAnimationId, Animation>> Cache =
            BuildAll();

        /// <summary>Mapa completo animação lógica → template para esta parte.</summary>
        public static IReadOnlyDictionary<CharacterAnimationId, Animation> GetForPart(CharacterPartSlot slot)
        {
            return Cache[slot];
        }

        private static int Linear(int row, int col) => row * SheetColumns + col;

        private static int IdleRow(CharacterPartSlot slot) => slot switch
        {
            CharacterPartSlot.Head => 0,
            CharacterPartSlot.Torso => 1,
            CharacterPartSlot.Arms => 2,
            CharacterPartSlot.Legs => 3,
            _ => 0,
        };

        private static int WalkRow(CharacterPartSlot slot) => slot switch
        {
            CharacterPartSlot.Head => 4,
            CharacterPartSlot.Torso => 5,
            CharacterPartSlot.Arms => 6,
            CharacterPartSlot.Legs => 7,
            _ => 4,
        };

        /// <summary>5 frames consecutivos a partir da coluna inicial.</summary>
        private static int[] IdleDir(int partRow, int colStart)
        {
            var a = new int[5];
            for (int i = 0; i < 5; i++)
            {
                a[i] = Linear(partRow, colStart + i);
            }

            return a;
        }

        /// <summary>6 frames consecutivos a partir da coluna inicial (walk).</summary>
        private static int[] WalkDir(int partRow, int colStart)
        {
            var a = new int[6];
            for (int i = 0; i < 6; i++)
            {
                a[i] = Linear(partRow, colStart + i);
            }

            return a;
        }

        private static Dictionary<CharacterPartSlot, IReadOnlyDictionary<CharacterAnimationId, Animation>> BuildAll()
        {
            var outer = new Dictionary<CharacterPartSlot, IReadOnlyDictionary<CharacterAnimationId, Animation>>();
            foreach (CharacterPartSlot slot in new[]
                     {
                         CharacterPartSlot.Legs,
                         CharacterPartSlot.Torso,
                         CharacterPartSlot.Arms,
                         CharacterPartSlot.Head,
                     })
            {
                outer[slot] = BuildForPart(slot);
            }

            return outer;
        }

        private static IReadOnlyDictionary<CharacterAnimationId, Animation> BuildForPart(CharacterPartSlot slot)
        {
            int ir = IdleRow(slot);
            int wr = WalkRow(slot);

            // Idle por direção (cols 0–4, 5–9, 10–14, 15–19)
            int[] idleDown = IdleDir(ir, 0);
            int[] idleUp = IdleDir(ir, 5);
            int[] idleLeft = IdleDir(ir, 10);
            int[] idleRight = IdleDir(ir, 15);

            // Walk por direção (cols 0–5, 6–11, 12–17, 18–23)
            int[] walkDown = WalkDir(wr, 0);
            int[] walkUp = WalkDir(wr, 6);
            int[] walkLeft = WalkDir(wr, 12);
            int[] walkRight = WalkDir(wr, 18);

            // Ataques / ferramenta / dano: placeholder = mesma pose que idle frente (5 frames) até haver arte dedicada
            int[] atkDown = IdleDir(ir, 0);
            int[] atkUp = IdleDir(ir, 5);
            int[] atkLeft = IdleDir(ir, 10);
            int[] atkRight = IdleDir(ir, 15);

            int[] tool = IdleDir(ir, 0);
            int[] hurt = new[] { Linear(ir, 0), Linear(ir, 1) };
            int[] death = IdleDir(ir, 0);

            return new Dictionary<CharacterAnimationId, Animation>
            {
                [CharacterAnimationId.IdleDown] = Animation.CreateTemplate("idle_down", idleDown, IdleFrameDuration, true),
                [CharacterAnimationId.IdleUp] = Animation.CreateTemplate("idle_up", idleUp, IdleFrameDuration, true),
                [CharacterAnimationId.IdleLeft] = Animation.CreateTemplate("idle_left", idleLeft, IdleFrameDuration, true),
                [CharacterAnimationId.IdleRight] = Animation.CreateTemplate("idle_right", idleRight, IdleFrameDuration, true),

                [CharacterAnimationId.WalkDown] = Animation.CreateTemplate("walk_down", walkDown, WalkFrameDuration, true),
                [CharacterAnimationId.WalkUp] = Animation.CreateTemplate("walk_up", walkUp, WalkFrameDuration, true),
                [CharacterAnimationId.WalkLeft] = Animation.CreateTemplate("walk_left", walkLeft, WalkFrameDuration, true),
                [CharacterAnimationId.WalkRight] = Animation.CreateTemplate("walk_right", walkRight, WalkFrameDuration, true),

                [CharacterAnimationId.AttackDown] = Animation.CreateTemplate("attack_down", atkDown, 0.08f, false),
                [CharacterAnimationId.AttackUp] = Animation.CreateTemplate("attack_up", atkUp, 0.08f, false),
                [CharacterAnimationId.AttackLeft] = Animation.CreateTemplate("attack_left", atkLeft, 0.08f, false),
                [CharacterAnimationId.AttackRight] = Animation.CreateTemplate("attack_right", atkRight, 0.08f, false),

                [CharacterAnimationId.ToolUse] = Animation.CreateTemplate("tool_use", tool, 0.12f, false),
                [CharacterAnimationId.Hurt] = Animation.CreateTemplate("hurt", hurt, 0.10f, false),
                [CharacterAnimationId.Death] = Animation.CreateTemplate("death", death, 0.14f, false),
            };
        }
    }
}
