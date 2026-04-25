namespace Embervalle.Core.Sprites
{
    /// <summary>Templates de <see cref="Animation"/> alinhados ao layout da ficha de jogador (índices no atlas).</summary>
    public static class PlayerAnimations
    {
        public static readonly Animation IdleDown =
            Animation.CreateTemplate("idle_down", new[] { 0, 1, 2, 3, 4 }, 0.32f, true);
        public static readonly Animation IdleUp =
            Animation.CreateTemplate("idle_up", new[] { 6, 7, 8, 9, 10 }, 0.32f, true);
        public static readonly Animation IdleLeft =
            Animation.CreateTemplate("idle_left", new[] { 12, 13, 14, 15, 16 }, 0.32f, true);
        public static readonly Animation IdleRight =
            Animation.CreateTemplate("idle_right", new[] { 18, 19, 20, 21, 22 }, 0.32f, true);
        public static readonly Animation WalkDown =
            Animation.CreateTemplate("walk_down", new[] { 24, 25, 26, 27, 28, 29 }, 0.10f, true);
        public static readonly Animation WalkUp =
            Animation.CreateTemplate("walk_up", new[] { 30, 31, 32, 33, 34, 35 }, 0.10f, true);
        public static readonly Animation WalkLeft =
            Animation.CreateTemplate("walk_left", new[] { 36, 37, 36, 39, 40, 41 }, 0.10f, true);
        public static readonly Animation WalkRight =
            Animation.CreateTemplate("walk_right", new[] { 42, 43, 44, 45, 46, 47 }, 0.10f, true);
        public static readonly Animation AttackDown =
            Animation.CreateTemplate("attack_down", new[] { 40, 41, 42, 43 }, 0.08f, false);
        public static readonly Animation AttackUp =
            Animation.CreateTemplate("attack_up", new[] { 44, 45, 46, 47 }, 0.08f, false);
        public static readonly Animation AttackLeft =
            Animation.CreateTemplate("attack_left", new[] { 48, 49, 50, 51 }, 0.08f, false);
        public static readonly Animation AttackRight =
            Animation.CreateTemplate("attack_right", new[] { 52, 53, 54, 55 }, 0.08f, false);
        public static readonly Animation ToolUse =
            Animation.CreateTemplate("tool_use", new[] { 56, 57, 58, 59 }, 0.12f, false);
        public static readonly Animation Hurt =
            Animation.CreateTemplate("hurt", new[] { 60, 61 }, 0.10f, false);
        public static readonly Animation Death =
            Animation.CreateTemplate("death", new[] { 62, 63, 64, 65 }, 0.14f, false);
    }
}
