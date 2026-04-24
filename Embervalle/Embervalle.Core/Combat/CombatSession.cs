using System.Collections.Generic;
using Embervalle.Core.Combat.Hostile;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.Sprites;
using Embervalle.Core.World;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    
    
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

        
        public bool IsAttackAnimationActive => _toolbarCombat.IsAttackAnimationActive;

        
        public bool IsMeleeSwingActive => _toolbarCombat.IsMeleeSwingActive;

        
        public int MeleeSwingCurrentFrame => _toolbarCombat.MeleeSwingCurrentFrame;

        public PlayerCardinalFacing MeleeSwingFacing => _toolbarCombat.MeleeSwingFacing;

        public int MeleeSwingWeaponIconFrame => _toolbarCombat.MeleeSwingWeaponIconFrame;

        public bool IsPlayerMovementLocked => _toolbarCombat.IsMovementLocked;

        public Vector2? GetAttackSpriteFaceDirection() => _toolbarCombat.GetAttackSpriteFaceDirection();

        public bool ShouldDrawMeleeWeaponOverlay => _toolbarCombat.ShouldDrawMeleeWeaponOverlay;

        public int MeleeWeaponOverlayDrawFrame => _toolbarCombat.GetMeleeWeaponOverlayFrame();

        
        public void ResetDemoTargets(int viewportWidth, int viewportHeight)
        {
            Enemies.Clear();
            Vector2 slimePos = new Vector2(viewportWidth * 0.25f, viewportHeight * 0.45f);
            Vector2 goblinPos = new Vector2(viewportWidth * 0.72f, viewportHeight * 0.55f);
            Enemies.Add(
                CombatEnemy.CreateHostile(
                    1,
                    slimePos,
                    32,
                    HostileEnemyProfile.Slime,
                    new[]
                    {
                        slimePos + new Vector2(-70f, 0f),
                        slimePos + new Vector2(70f, 0f),
                    }));
            Enemies.Add(
                CombatEnemy.CreateHostile(
                    2,
                    goblinPos,
                    60,
                    HostileEnemyProfile.Goblin,
                    new[]
                    {
                        goblinPos + new Vector2(0f, -60f),
                        goblinPos + new Vector2(0f, 60f),
                    }));
        }

        public void Update(
            PlayerBody player,
            Camera2D camera,
            InputManager input,
            float deltaSeconds,
            ToolbarSlots toolbar,
            int selectedToolbarSlotIndex,
            PlayerCardinalFacing combatFacing,
            int viewportWidth,
            int viewportHeight)
        {
            HostileEnemyAiSystem.Update(Enemies, player, viewportWidth, viewportHeight, false, deltaSeconds);
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
