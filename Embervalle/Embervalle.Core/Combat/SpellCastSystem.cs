using System;
using System.Collections.Generic;
using Embervalle.Core.Events;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public sealed class SpellCastSystem
    {
        private readonly ProjectilePool _pool;

        private readonly int _playerOwnerId;

        public SpellCastSystem(ProjectilePool pool, int playerOwnerId)
        {
            _pool = pool;
            _playerOwnerId = playerOwnerId;
        }

        public bool TryCast(
            SpellData spell,
            Vector2 casterFeetPosition,
            Vector2 aimDirNormalized,
            Vector2 aimWorldPos,
            ManaComponent mana,
            CooldownComponent cooldowns,
            IReadOnlyList<CombatEnemy> enemies)
        {
            if (mana.Current < spell.ManaCost)
            {
                return false;
            }

            if (cooldowns.IsOnCooldown(spell.SpellId))
            {
                return false;
            }

            mana.Current -= spell.ManaCost;
            cooldowns.StartCooldown(spell.SpellId, spell.Cooldown);

            switch (spell.Type)
            {
                case SpellType.Projectile:
                    if (aimDirNormalized.LengthSquared() < 0.0001f)
                    {
                        aimDirNormalized = Vector2.UnitX;
                    }
                    else
                    {
                        aimDirNormalized = Vector2.Normalize(aimDirNormalized);
                    }

                    _ = _pool.Spawn(
                        casterFeetPosition,
                        aimDirNormalized,
                        spell.ProjectileSpeed,
                        spell.Damage,
                        spell.ProjectileRange,
                        gravity: 0f,
                        ownerId: _playerOwnerId,
                        ProjectileKind.Spell,
                        spell);
                    break;

                case SpellType.AreaMelee:
                    MeleeCombat.ExecuteMeleeAttack(
                        casterFeetPosition,
                        aimDirNormalized,
                        CreateAreaMeleeWeapon(spell),
                        enemies);
                    break;

                case SpellType.AreaFixed:
                    ApplyAreaFixed(aimWorldPos, spell);
                    break;

                case SpellType.Self:
                    break;

                case SpellType.Movement:
                    break;
            }

            EventBus.Publish(new SpellCastEvent { Spell = spell });
            return true;
        }

        private static void ApplyAreaFixed(Vector2 aimWorldPos, SpellData spell)
        {
            _ = aimWorldPos;
            _ = spell;
        }

        private static WeaponData CreateAreaMeleeWeapon(SpellData spell)
        {
            return new WeaponData
            {
                Id = spell.SpellId + "_area_melee",
                Kind = WeaponKind.Melee,
                Damage = spell.Damage,
                Range = spell.AreaRadius > 0f ? spell.AreaRadius : 80f,
                ArcHalfAngleRadians = MathF.PI * 135f / 180f,
                Knockback = 4f,
                CooldownSeconds = 0f,
                BowChargeSecondsMax = 0f,
                BowBaseDamage = 0,
                BowProjectileSpeed = 0f,
                BowMaxRange = 0f,
                ChargeDamageBonusMax = 0f,
            };
        }
    }
}
