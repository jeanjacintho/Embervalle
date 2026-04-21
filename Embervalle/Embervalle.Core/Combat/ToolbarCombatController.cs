using System;
using System.Collections.Generic;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public sealed class ToolbarCombatController
    {
        private readonly float[] _toolbarCooldownRemaining;

        private bool _swingActive;
        private int _swingFrame;
        private float _swingFrameTimer;
        private float _followThroughTimer;
        private PlayerCardinalFacing _swingFacing;
        private WeaponData? _swingWeapon;
        private int _swingIconFrame;
        private readonly HashSet<int> _swingHitEnemies = new();

        private int _meleeWeaponDisplayFrame;

        private bool _lockMeleeSpriteFacing;
        private PlayerCardinalFacing _lockedSpriteFacing;

        private bool _bowNocking;
        private int _bowSlotIndex;
        private float _bowChargeTime;

        private float _attackAnimTimer;

        public ToolbarCombatController(int toolbarSlotCount)
        {
            _toolbarCooldownRemaining = new float[toolbarSlotCount];
        }

        public bool IsAttackAnimationActive => _attackAnimTimer > 0f;

        public bool IsMeleeSwingActive => _swingActive;

        public int MeleeSwingCurrentFrame => _swingFrame;

        public PlayerCardinalFacing MeleeSwingFacing => _swingFacing;

        public int MeleeSwingWeaponIconFrame => _swingIconFrame;

        public bool IsMovementLocked => _swingActive || _followThroughTimer > 0f;

        public Vector2? GetAttackSpriteFaceDirection()
        {
            if (_attackAnimTimer <= 0f || !_lockMeleeSpriteFacing)
            {
                return null;
            }

            return MeleeFacingVectors.ToWorldUnit(_lockedSpriteFacing);
        }

        public bool ShouldDrawMeleeWeaponOverlay =>
            _swingActive || (_lockMeleeSpriteFacing && _attackAnimTimer > 0f);

        public int GetMeleeWeaponOverlayFrame() =>
            Math.Min(_meleeWeaponDisplayFrame, MeleeSwingFrameTable.FrameCount - 1);

        public void Update(
            float dt,
            PlayerBody player,
            InputManager input,
            ToolbarSlots toolbar,
            int selectedSlotIndex,
            PlayerCardinalFacing combatFacing,
            RangedAimSystem rangedAim,
            ProjectilePool projectiles,
            List<CombatEnemy> enemies)
        {
            AdvanceCooldowns(dt);
            AdvanceAttackAnimAndMeleeFacingLock(dt);
            AdvanceSwingFrames(dt, player, enemies);

            if (_swingActive)
            {
                return;
            }

            if (_bowNocking)
            {
                ItemSlot bowSlot = toolbar.GetSlot(_bowSlotIndex);
                if (!CombatGearResolver.TryGetWeapon(bowSlot, out WeaponData? bowWeapon)
                    || bowWeapon is null
                    || bowWeapon.Kind != WeaponKind.Ranged)
                {
                    _bowNocking = false;
                    _bowChargeTime = 0f;
                }
                else
                {
                    UpdateBowHeld(player, bowWeapon, _bowSlotIndex, input, dt, rangedAim, projectiles);
                }

                return;
            }

            int slotIndex = Math.Clamp(selectedSlotIndex, 0, toolbar.SlotCount - 1);

            if (TryStartBowNock(input, toolbar, slotIndex))
            {
                return;
            }

            TryMeleeOrConsumable(player, input, toolbar, slotIndex, combatFacing, enemies);
        }

        private void AdvanceCooldowns(float dt)
        {
            for (int s = 0; s < _toolbarCooldownRemaining.Length; s++)
            {
                _toolbarCooldownRemaining[s] = MathHelper.Max(0f, _toolbarCooldownRemaining[s] - dt);
            }
        }

        private void AdvanceAttackAnimAndMeleeFacingLock(float dt)
        {
            if (_attackAnimTimer > 0f)
            {
                _attackAnimTimer = MathHelper.Max(0f, _attackAnimTimer - dt);
                if (_attackAnimTimer <= 0f)
                {
                    _lockMeleeSpriteFacing = false;
                }
            }

            if (_followThroughTimer > 0f)
            {
                _followThroughTimer = MathHelper.Max(0f, _followThroughTimer - dt);
            }
        }

        private void AdvanceSwingFrames(float dt, PlayerBody player, List<CombatEnemy> enemies)
        {
            if (!_swingActive)
            {
                return;
            }

            _swingFrameTimer -= dt;
            int guard = 0;
            while (_swingFrameTimer <= 0f && _swingActive && guard < MeleeSwingTiming.MaxSwingCatchUpStepsPerFrame)
            {
                guard++;
                if (_swingWeapon != null)
                {
                    Rectangle hitbox = MeleeSwingFrameTable.GetHitbox(
                        _swingFacing,
                        _swingFrame,
                        player.FeetPosition);
                    MeleeCombat.ApplyFrameHit(hitbox, _swingWeapon, enemies, _swingHitEnemies);
                    _meleeWeaponDisplayFrame = _swingFrame;
                }

                _swingFrame++;

                if (_swingFrame >= MeleeSwingFrameTable.FrameCount)
                {
                    _swingActive = false;
                    _meleeWeaponDisplayFrame = MeleeSwingFrameTable.FrameCount - 1;
                    _followThroughTimer = MeleeSwingTiming.FollowThroughSeconds;
                }
                else
                {
                    _swingFrameTimer += MeleeSwingTiming.FrameDurationSeconds;
                }
            }
        }

        private bool TryStartBowNock(InputManager input, ToolbarSlots toolbar, int slotIndex)
        {
            if (!input.AttackJustPressed)
            {
                return false;
            }

            ItemSlot slot = toolbar.GetSlot(slotIndex);
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
            InputManager input,
            ToolbarSlots toolbar,
            int slotIndex,
            PlayerCardinalFacing combatFacing,
            List<CombatEnemy> enemies)
        {
            if (!input.AttackJustPressed)
            {
                return;
            }

            ItemSlot slot = toolbar.GetSlot(slotIndex);
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

            if (_toolbarCooldownRemaining[slotIndex] > 0f)
            {
                return;
            }

            _swingActive = true;
            _swingFrame = 1;
            _swingFrameTimer = MeleeSwingTiming.FrameDurationSeconds;
            _swingFacing = combatFacing;
            _swingWeapon = w;
            _swingIconFrame = data.IconAtlasFrameIndex >= 0 ? data.IconAtlasFrameIndex : 0;
            _swingHitEnemies.Clear();
            _followThroughTimer = 0f;

            _meleeWeaponDisplayFrame = 0;
            _lockMeleeSpriteFacing = true;
            _lockedSpriteFacing = combatFacing;

            _toolbarCooldownRemaining[slotIndex] = w.CooldownSeconds;
            float swingSeconds = MeleeSwingFrameTable.FrameCount * MeleeSwingTiming.FrameDurationSeconds;
            _attackAnimTimer = swingSeconds + MeleeSwingTiming.FollowThroughSeconds;

            Rectangle firstHitbox = MeleeSwingFrameTable.GetHitbox(_swingFacing, 0, player.FeetPosition);
            MeleeCombat.ApplyFrameHit(firstHitbox, w, enemies, _swingHitEnemies);
        }

        private void UpdateBowHeld(
            PlayerBody player,
            WeaponData weapon,
            int slotIndex,
            InputManager input,
            float dt,
            RangedAimSystem rangedAim,
            ProjectilePool projectiles)
        {
            if (input.AttackHeld)
            {
                _bowChargeTime += dt;
                float max = weapon.BowChargeSecondsMax > 0f ? weapon.BowChargeSecondsMax : 1.5f;
                _bowChargeTime = MathHelper.Min(_bowChargeTime, max);
            }

            if (!input.AttackJustReleased)
            {
                return;
            }

            if (_toolbarCooldownRemaining[slotIndex] <= 0f)
            {
                float maxCharge = weapon.BowChargeSecondsMax > 0f ? weapon.BowChargeSecondsMax : 1.5f;
                float t = MathHelper.Clamp(_bowChargeTime / maxCharge, 0f, 1f);
                int damage = (int)(weapon.BowBaseDamage * (1f + t * weapon.ChargeDamageBonusMax));
                Vector2 dir = rangedAim.AimDirection;
                if (dir.LengthSquared() < 0.0001f)
                {
                    dir = Vector2.UnitX;
                }
                else
                {
                    dir = Vector2.Normalize(dir);
                }

                ProjectileState? arrow = projectiles.Spawn(
                    player.FeetPosition,
                    dir,
                    weapon.BowProjectileSpeed,
                    damage,
                    weapon.BowMaxRange,
                    gravity: 0.12f,
                    ownerId: 0,
                    ProjectileKind.Arrow);

                if (arrow != null)
                {
                    _toolbarCooldownRemaining[slotIndex] = weapon.CooldownSeconds;
                    _attackAnimTimer = 0.2f;
                }
            }

            _bowNocking = false;
            _bowChargeTime = 0f;
        }
    }
}
