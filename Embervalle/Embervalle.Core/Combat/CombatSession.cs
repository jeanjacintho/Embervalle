using System;
using System.Collections.Generic;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.World;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Orquestra apontamento, armas, pool de projéteis e alvos — um passo por frame no Update.</summary>
    public sealed class CombatSession
    {
        public const int PlayerOwnerId = 0;

        public readonly AimSystem Aim = new();

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

        private readonly float[] _quickCooldownRemaining = new float[QuickAccessSlots.SlotCountValue];

        private bool _bowNocking;

        private int _bowSlotIndex;

        private float _bowChargeTime;

        private float _attackAnimTimer;

        public bool IsAttackAnimationActive => _attackAnimTimer > 0f;

        public CombatSession()
        {
            Projectiles = new ProjectilePool(64);
            ProjectilePhysics = new ProjectilePhysicsSystem(Projectiles);
            SpellCast = new SpellCastSystem(Projectiles, PlayerOwnerId);
            Equipment.SpellSlot1 = SpellDefinitions.Fireball;
        }

        public void ResetDemoTargets(int viewportWidth, int viewportHeight)
        {
            Enemies.Clear();
            Enemies.Add(CombatEnemy.Create(
                1,
                new Vector2(viewportWidth * 0.25f, viewportHeight * 0.45f),
                80));
            Enemies.Add(CombatEnemy.Create(
                2,
                new Vector2(viewportWidth * 0.72f, viewportHeight * 0.55f),
                80));
        }

        public void Update(
            PlayerBody player,
            Camera2D camera,
            InputManager input,
            float deltaSeconds,
            QuickAccessSlots quickAccess,
            int selectedQuickSlotIndex)
        {
            int slotIndex = Math.Clamp(selectedQuickSlotIndex, 0, 1);

            Mana.Update(deltaSeconds);
            Cooldowns.Update(deltaSeconds);
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Health.UpdateInvincibility(deltaSeconds);
            }

            for (int s = 0; s < _quickCooldownRemaining.Length; s++)
            {
                _quickCooldownRemaining[s] = MathHelper.Max(0f, _quickCooldownRemaining[s] - deltaSeconds);
            }

            Aim.Update(player.FeetPosition, camera, input);

            if (_attackAnimTimer > 0f)
            {
                _attackAnimTimer = MathHelper.Max(0f, _attackAnimTimer - deltaSeconds);
            }

            if (input.Spell1JustPressed && Equipment.SpellSlot1 != null)
            {
                _ = SpellCast.TryCast(
                    Equipment.SpellSlot1,
                    player.FeetPosition,
                    Aim.AimDirection,
                    Aim.AimWorldPosition,
                    Mana,
                    Cooldowns,
                    Enemies);
            }

            if (_bowNocking)
            {
                ItemSlot bowSlot = quickAccess.GetSlot(_bowSlotIndex);
                if (!CombatGearResolver.TryGetWeapon(bowSlot, out WeaponData? bowWeapon)
                    || bowWeapon.Kind != WeaponKind.Ranged)
                {
                    _bowNocking = false;
                    _bowChargeTime = 0f;
                }
                else
                {
                    UpdateBowHeld(player, bowWeapon, _bowSlotIndex, input, deltaSeconds);
                }
            }
            else
            {
                if (TryStartBowNock(input, quickAccess, slotIndex))
                {
                    // arco: inicia com clique; carga no proximo frame
                }
                else
                {
                    TryMeleeOrConsumable(player, quickAccess, slotIndex, input);
                }
            }

            ProjectilePhysics.Update(Enemies, deltaSeconds);
        }

        private bool TryStartBowNock(InputManager input, QuickAccessSlots quick, int slotIndex)
        {
            if (!input.AttackJustPressed)
            {
                return false;
            }

            ItemSlot slot = quick.GetSlot(slotIndex);
            if (!CombatGearResolver.TryGetWeapon(slot, out WeaponData? w) || w == null || w.Kind != WeaponKind.Ranged)
            {
                return false;
            }

            _bowNocking = true;
            _bowSlotIndex = slotIndex;
            _bowChargeTime = 0f;
            return true;
        }

        private void TryMeleeOrConsumable(
            PlayerBody player,
            QuickAccessSlots quick,
            int slotIndex,
            InputManager input)
        {
            if (!input.AttackJustPressed)
            {
                return;
            }

            ItemSlot slot = quick.GetSlot(slotIndex);
            if (slot.IsEmpty)
            {
                return;
            }

            ItemData data = slot.Item!.GetData();
            if (data.Category == ItemCategory.Consumable)
            {
                _ = ConsumableUse.TryUse(slot, player);
                return;
            }

            if (!CombatGearResolver.TryGetWeapon(slot, out WeaponData? w) || w == null || w.Kind != WeaponKind.Melee)
            {
                return;
            }

            if (_quickCooldownRemaining[slotIndex] > 0f)
            {
                return;
            }

            MeleeCombat.ExecuteMeleeAttack(
                player.FeetPosition,
                Aim.AimDirection,
                w,
                Enemies);
            _quickCooldownRemaining[slotIndex] = w.CooldownSeconds;
            _attackAnimTimer = 0.2f;
        }

        private void UpdateBowHeld(
            PlayerBody player,
            WeaponData weapon,
            int slotIndex,
            InputManager input,
            float deltaSeconds)
        {
            if (input.AttackHeld)
            {
                _bowChargeTime += deltaSeconds;
                float max = weapon.BowChargeSecondsMax > 0f ? weapon.BowChargeSecondsMax : 1.5f;
                _bowChargeTime = MathHelper.Min(_bowChargeTime, max);
            }

            if (!input.AttackJustReleased)
            {
                return;
            }

            if (_quickCooldownRemaining[slotIndex] <= 0f)
            {
                float maxCharge = weapon.BowChargeSecondsMax > 0f ? weapon.BowChargeSecondsMax : 1.5f;
                float t = MathHelper.Clamp(_bowChargeTime / maxCharge, 0f, 1f);
                int damage = (int)(weapon.BowBaseDamage * (1f + t * weapon.ChargeDamageBonusMax));
                Vector2 dir = Aim.AimDirection;
                if (dir.LengthSquared() < 0.0001f)
                {
                    dir = Vector2.UnitX;
                }
                else
                {
                    dir = Vector2.Normalize(dir);
                }

                ProjectileState? arrow = Projectiles.Spawn(
                    player.FeetPosition,
                    dir,
                    weapon.BowProjectileSpeed,
                    damage,
                    weapon.BowMaxRange,
                    gravity: 0.12f,
                    ownerId: PlayerOwnerId,
                    ProjectileKind.Arrow);

                if (arrow != null)
                {
                    _quickCooldownRemaining[slotIndex] = weapon.CooldownSeconds;
                    _attackAnimTimer = 0.2f;
                }
            }

            _bowNocking = false;
            _bowChargeTime = 0f;
        }
    }
}
