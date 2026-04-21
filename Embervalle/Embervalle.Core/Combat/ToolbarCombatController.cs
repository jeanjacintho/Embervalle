using System;
using System.Collections.Generic;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.Sprites;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>
    /// Uso do item da toolbar (padrão Stardew Valley).
    /// Melee segue <see cref="PlayerCardinalFacing"/> com swing de <see cref="MeleeSwingFrameTable.FrameCount"/>
    /// frames discretos, cada um com hitbox própria e dano aplicado por frame.
    /// </summary>
    public sealed class ToolbarCombatController
    {
        // ──────────────────────────────────────────────
        //  Duração de cada frame do swing (SDV ≈ 50ms/frame a 8fps → ~400ms total / 6 frames ≈ 67ms)
        private const float FrameDurationSeconds = 0.065f;

        // Pausa de follow-through após o último frame antes de poder atacar novamente
        private const float FollowThroughSeconds = 0.12f;
        // ──────────────────────────────────────────────

        private readonly float[] _toolbarCooldownRemaining;

        // ── Estado do swing melee ──
        private bool _swingActive;
        private int _swingFrame;
        private float _swingFrameTimer;
        private float _followThroughTimer;
        private PlayerCardinalFacing _swingFacing;
        private WeaponData? _swingWeapon;
        private int _swingIconFrame;
        private readonly HashSet<int> _swingHitEnemies = new();

        // ── Estado do arco ──
        private bool _bowNocking;
        private int _bowSlotIndex;
        private float _bowChargeTime;

        // ── Animação de ataque (feedback ao PlayerAnimController) ──
        private float _attackAnimTimer;

        public ToolbarCombatController(int toolbarSlotCount)
        {
            _toolbarCooldownRemaining = new float[toolbarSlotCount];
        }

        // ──────────────── Propriedades públicas ────────────────

        /// <summary>Indica que está em animação de ataque (melee ou arco).</summary>
        public bool IsAttackAnimationActive => _attackAnimTimer > 0f;

        /// <summary>Indica que o swing melee está ativo (arma deve ser desenhada).</summary>
        public bool IsMeleeSwingActive => _swingActive;

        /// <summary>Frame atual do swing (0 a <see cref="MeleeSwingFrameTable.FrameCount"/> − 1).</summary>
        public int MeleeSwingCurrentFrame => _swingFrame;

        public PlayerCardinalFacing MeleeSwingFacing => _swingFacing;

        public int MeleeSwingWeaponIconFrame => _swingIconFrame;

        /// <summary>
        /// Movimento do jogador deve ser bloqueado durante o swing melee (igual SDV: UsingTool=true).
        /// </summary>
        public bool IsMovementLocked => _swingActive || _followThroughTimer > 0f;

        // ──────────────── Update principal ────────────────

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
            AdvanceSwingFrames(dt, player, enemies);
            AdvanceAttackAnim(dt);

            if (_swingActive)
            {
                // Jogador está em pleno swing: não processa nova ação
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

        // ──────────────── Internos ────────────────

        private void AdvanceCooldowns(float dt)
        {
            for (int s = 0; s < _toolbarCooldownRemaining.Length; s++)
            {
                _toolbarCooldownRemaining[s] = MathHelper.Max(0f, _toolbarCooldownRemaining[s] - dt);
            }
        }

        private void AdvanceAttackAnim(float dt)
        {
            if (_attackAnimTimer > 0f)
            {
                _attackAnimTimer = MathHelper.Max(0f, _attackAnimTimer - dt);
            }

            if (_followThroughTimer > 0f)
            {
                _followThroughTimer = MathHelper.Max(0f, _followThroughTimer - dt);
            }
        }

        /// <summary>
        /// Avança frames do swing melee e aplica hitbox por frame (padrão SDV).
        /// Cada frame tem posição e rotação da arma; dano é verificado a cada frame.
        /// </summary>
        private void AdvanceSwingFrames(float dt, PlayerBody player, List<CombatEnemy> enemies)
        {
            if (!_swingActive)
            {
                return;
            }

            _swingFrameTimer -= dt;

            while (_swingFrameTimer <= 0f && _swingActive)
            {
                // Aplica hit do frame atual antes de avançar
                if (_swingWeapon != null)
                {
                    Rectangle hitbox = MeleeSwingFrameTable.GetHitbox(
                        _swingFacing, _swingFrame, player.FeetPosition);
                    MeleeCombat.ApplyFrameHit(hitbox, _swingWeapon, enemies, _swingHitEnemies);
                }

                _swingFrame++;

                if (_swingFrame >= MeleeSwingFrameTable.FrameCount)
                {
                    _swingActive = false;
                    _followThroughTimer = FollowThroughSeconds;
                }
                else
                {
                    _swingFrameTimer += FrameDurationSeconds;
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

            // Inicia swing melee (padrão SDV: setFarmerAnimating → doSwipe)
            _swingActive = true;
            _swingFrame = 0;
            _swingFrameTimer = FrameDurationSeconds;
            _swingFacing = combatFacing;
            _swingWeapon = w;
            _swingIconFrame = data.IconAtlasFrameIndex >= 0 ? data.IconAtlasFrameIndex : 0;
            _swingHitEnemies.Clear();
            _followThroughTimer = 0f;

            _toolbarCooldownRemaining[slotIndex] = w.CooldownSeconds;
            _attackAnimTimer = MeleeSwingFrameTable.FrameCount * FrameDurationSeconds + FollowThroughSeconds;

            // Hit imediato no frame 0 (SDV também chama DoDamage no início do swing)
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
