using System.Collections.Generic;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.Sprites;
using Embervalle.Core.World;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>
    /// Estado de combate no mundo: alvos, projéteis, mana e orquestração.
    /// O uso de itens da toolbar (arma melee, consumível, arco) está em <see cref="ToolbarCombatController"/>.
    /// </summary>
    public sealed class CombatSession
    {
        public const int PlayerOwnerId = 0;

        public readonly RangedAimSystem RangedAim = new();

        public readonly EquipmentComponent Equipment = new();

        public readonly ManaComponent Mana = new()
        {
            Current = 100f,
            Max = 100f,
            RegenPerSecond = 8f,
        };

        public readonly CooldownComponent Cooldowns = new();

        public readonly List<CombatEnemy> Enemies = new();

        public readonly ProjectilePool Projectiles;

        public readonly ProjectilePhysicsSystem ProjectilePhysics;

        public readonly SpellCastSystem SpellCast;

        private readonly ToolbarCombatController _toolbarCombat;

        public CombatSession()
        {
            Projectiles = new ProjectilePool(64);
            ProjectilePhysics = new ProjectilePhysicsSystem(Projectiles);
            SpellCast = new SpellCastSystem(Projectiles, PlayerOwnerId);
            Equipment.SpellSlot1 = SpellDefinitions.Fireball;
            _toolbarCombat = new ToolbarCombatController(ToolbarSlots.SlotCountValue);
        }

        // ──────────── Propriedades expostas ao EmbervalleGame ────────────

        /// <summary>Em animação de ataque (melee ou arco) — drive da animação do personagem.</summary>
        public bool IsAttackAnimationActive => _toolbarCombat.IsAttackAnimationActive;

        /// <summary>Swing melee ativo — o sprite da arma deve ser desenhado.</summary>
        public bool IsMeleeSwingActive => _toolbarCombat.IsMeleeSwingActive;

        /// <summary>Frame atual do swing (0 a <see cref="MeleeSwingFrameTable.FrameCount"/> − 1).</summary>
        public int MeleeSwingCurrentFrame => _toolbarCombat.MeleeSwingCurrentFrame;

        public PlayerCardinalFacing MeleeSwingFacing => _toolbarCombat.MeleeSwingFacing;

        public int MeleeSwingWeaponIconFrame => _toolbarCombat.MeleeSwingWeaponIconFrame;

        /// <summary>
        /// O jogador não pode mover durante o swing (igual SDV: UsingTool=true / CanMove=false).
        /// </summary>
        public bool IsPlayerMovementLocked => _toolbarCombat.IsMovementLocked;

        // ──────────────────────────────────────────────────────

        public void ResetDemoTargets(int viewportWidth, int viewportHeight)
        {
            Enemies.Clear();
            Enemies.Add(CombatEnemy.Create(1,new Vector2(viewportWidth * 0.25f, viewportHeight * 0.45f),800));
            Enemies.Add(CombatEnemy.Create(2,new Vector2(viewportWidth * 0.72f, viewportHeight * 0.55f),800));
        }

        public void Update(
            PlayerBody player,
            Camera2D camera,
            InputManager input,
            float deltaSeconds,
            ToolbarSlots toolbar,
            int selectedToolbarSlotIndex,
            PlayerCardinalFacing combatFacing)
        {
            Mana.Update(deltaSeconds);
            Cooldowns.Update(deltaSeconds);
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Health.UpdateInvincibility(deltaSeconds);
            }

            RangedAim.Update(player.FeetPosition, camera, input);

            if (input.Spell1JustPressed && Equipment.SpellSlot1 != null)
            {
                _ = SpellCast.TryCast(
                    Equipment.SpellSlot1,
                    player.FeetPosition,
                    RangedAim.AimDirection,
                    RangedAim.AimWorldPosition,
                    Mana,
                    Cooldowns,
                    Enemies);
            }

            _toolbarCombat.Update(
                deltaSeconds,
                player,
                input,
                toolbar,
                selectedToolbarSlotIndex,
                combatFacing,
                RangedAim,
                Projectiles,
                Enemies);

            ProjectilePhysics.Update(Enemies, deltaSeconds);
        }
    }
}
