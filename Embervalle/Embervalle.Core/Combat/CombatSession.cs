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
    /// <summary>Estado de combate: inimigos, mira, equipamento, mana, armas, pool de projéteis e <see cref="HostileEnemyAiSystem"/>.</summary>
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

        /// <summary>Constrói pools, física de projéteis, lançamento de feitiços e slot de feitiço predefinido.</summary>
        public CombatSession()
        {
            Projectiles = new ProjectilePool(64);
            ProjectilePhysics = new ProjectilePhysicsSystem(Projectiles);
            SpellCast = new SpellCastSystem(Projectiles, PlayerOwnerId);
            Equipment.SpellSlot1 = SpellDefinitions.Fireball;
            _toolbarCombat = new ToolbarCombatController(ToolbarSlots.SlotCountValue);
        }

        /// <summary>Indica se a animação de ataque (corpo) está ativa.</summary>
        public bool IsAttackAnimationActive => _toolbarCombat.IsAttackAnimationActive;
        /// <summary>Indica se o arco de ataque corpo a corpo está a decorrer.</summary>
        public bool IsMeleeSwingActive => _toolbarCombat.IsMeleeSwingActive;
        /// <summary>Frame atual do arco de melée para sincronizar hitboxes.</summary>
        public int MeleeSwingCurrentFrame => _toolbarCombat.MeleeSwingCurrentFrame;
        /// <summary>Orientação cardinal do arco de melée.</summary>
        public PlayerCardinalFacing MeleeSwingFacing => _toolbarCombat.MeleeSwingFacing;
        /// <summary>Índice de frame do ícone de arma usado no swing.</summary>
        public int MeleeSwingWeaponIconFrame => _toolbarCombat.MeleeSwingWeaponIconFrame;
        /// <summary>Trava o movimento do jogador durante certas fases do ataque.</summary>
        public bool IsPlayerMovementLocked => _toolbarCombat.IsMovementLocked;
        /// <summary>Direção a usar para o sprite de ataque, se aplicável.</summary>
        public Vector2? GetAttackSpriteFaceDirection() => _toolbarCombat.GetAttackSpriteFaceDirection();
        /// <summary>Se deve desenhar a sobreposição de arma de melée.</summary>
        public bool ShouldDrawMeleeWeaponOverlay => _toolbarCombat.ShouldDrawMeleeWeaponOverlay;
        /// <summary>Frame a usar no overlay de arma de melée.</summary>
        public int MeleeWeaponOverlayDrawFrame => _toolbarCombat.GetMeleeWeaponOverlayFrame();

        /// <summary>Recria o slime e o goblin de teste (patrulhas e perfis de <see cref="HostileEnemyProfile"/>).</summary>
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

        /// <summary>Atualiza IA, mana, arrefecimentos, mira, feitiço, toolbar/melée e projéteis.</summary>
        /// <param name="viewportWidth">Limites horizontais para a IA inimiga e o jogador.</param>
        /// <param name="viewportHeight">Idem, vertical.</param>
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
