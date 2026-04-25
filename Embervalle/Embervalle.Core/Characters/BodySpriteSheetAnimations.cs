using System.Collections.Generic;
using Embervalle.Core.Sprites;

namespace Embervalle.Core.Characters
{
    /// <summary>Define as animações de idle, caminhada e ataque para cada camada do sprite de corpo humanoide.</summary>
    public static class BodySpriteSheetAnimations
    {
        public const int SheetColumns = 24;

        public const int FrameWidth = 48;

        public const int FrameHeight = 64;

        private const float IdleFrameDuration = 0.32f;

        private const float WalkFrameDuration = 0.10f;

        /// <summary>Cache de animações para todas as partes do corpo.</summary>
        private static readonly IReadOnlyDictionary<CharacterPartSlot, IReadOnlyDictionary<CharacterAnimationId, Animation>> Cache = BuildAll();

        /// <summary>Obtém as animações disponíveis para uma parte do corpo conforme o slot.</summary>
        public static IReadOnlyDictionary<CharacterAnimationId, Animation> GetForPart(CharacterPartSlot slot)
        {
            return Cache[slot];
        }

        private static int Linear(int row, int col) => row * SheetColumns + col;

        /// <summary>Obtém a linha de animação de idle para uma parte do corpo conforme o slot.</summary>
        private static int IdleRow(CharacterPartSlot slot) => slot switch
        {
            CharacterPartSlot.Head => 0,
            CharacterPartSlot.Torso => 1,
            CharacterPartSlot.Arms => 2,
            CharacterPartSlot.Legs => 3,
            _ => 0,
        };

        /// <summary>Obtém a linha de animação de walk para uma parte do corpo conforme o slot.</summary>
        private static int WalkRow(CharacterPartSlot slot) => slot switch
        {
            CharacterPartSlot.Head => 4,
            CharacterPartSlot.Torso => 5,
            CharacterPartSlot.Arms => 6,
            CharacterPartSlot.Legs => 7,
            _ => 4,
        };

        /// <summary>Obtém os índices das animações de idle para uma parte do corpo conforme a linha e coluna inicial.</summary>
        private static int[] IdleDir(int partRow, int colStart)
        {
            var a = new int[5];
            for (int i = 0; i < 5; i++)
            {
                a[i] = Linear(partRow, colStart + i);
            }

            return a;
        }

        /// <summary>Obtém os índices das animações de walk para uma parte do corpo conforme a linha e coluna inicial.</summary>
        private static int[] WalkDir(int partRow, int colStart)
        {
            var a = new int[6];
            for (int i = 0; i < 6; i++)
            {
                a[i] = Linear(partRow, colStart + i);
            }

            return a;
        }

        /// <summary>Constrói um dicionário de animações para todas as partes do corpo.</summary>
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

        /// <summary>Constrói um dicionário de animações para uma parte do corpo.</summary>
        private static IReadOnlyDictionary<CharacterAnimationId, Animation> BuildForPart(CharacterPartSlot slot)
        {
            int ir = IdleRow(slot);
            int wr = WalkRow(slot);

            
            int[] idleDown = IdleDir(ir, 0);
            int[] idleUp = IdleDir(ir, 5);
            int[] idleLeft = IdleDir(ir, 10);
            int[] idleRight = IdleDir(ir, 15);

            
            int[] walkDown = WalkDir(wr, 0);
            int[] walkUp = WalkDir(wr, 6);
            int[] walkLeft = WalkDir(wr, 12);
            int[] walkRight = WalkDir(wr, 18);

            
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
