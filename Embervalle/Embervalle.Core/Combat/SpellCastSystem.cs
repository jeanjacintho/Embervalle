using System;
using System.Collections.Generic;
using Embervalle.Core.Events;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Sistema de lançamento de feitiços: verifica mana e recarga, despacha projéteis ou ataques de área.</summary>
    public sealed class SpellCastSystem
    {
        private readonly ProjectilePool _pool;

        private readonly int _playerOwnerId;

        /// <summary>Usa o pool dado e o id de dono para projéteis do jogador.</summary>
        public SpellCastSystem(ProjectilePool pool, int playerOwnerId)
        {
            _pool = pool;
            _playerOwnerId = playerOwnerId;
        }

        /// <summary>Se mana e recarga permitirem, aplica o feitiço (projétil, melée de área, etc.) e publica <see cref="SpellCastEvent"/>.</summary>
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

        /// <summary>Placeholder para feitiço de área fixa no chão.</summary>
        private static void ApplyAreaFixed(Vector2 aimWorldPos, SpellData spell)
        {
            _ = aimWorldPos;
            _ = spell;
        }

        /// <summary>Cria um <see cref="WeaponData"/> sintético para reutilizar <see cref="MeleeCombat.ExecuteMeleeAttack"/>.</summary>
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
