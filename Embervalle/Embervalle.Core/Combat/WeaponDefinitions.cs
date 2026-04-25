using System;
using System.Collections.Generic;

namespace Embervalle.Core.Combat
{
    /// <summary>Catálogo estático de todas as armas definidas no jogo, indexadas por ID.</summary>
    public static class WeaponDefinitions
    {
        private static readonly Dictionary<string, WeaponData> ById = new();

        /// <summary>Regista as armas predefinidas no dicionário estático.</summary>
        static WeaponDefinitions()
        {
            Register(BasicSword);
            Register(Axe);
            Register(ShortBow);
        }

        /// <summary>Adiciona ou substitui a entrada pelo id da arma.</summary>
        private static void Register(WeaponData weapon) => ById[weapon.Id] = weapon;

        /// <summary>Resolve a definição de arma por id ou null se não existir.</summary>
        public static WeaponData? TryGet(string id) =>
            ById.TryGetValue(id, out WeaponData? w) ? w : null;

        public static readonly WeaponData BasicSword = new()
        {
            Id = "sword_basic",
            Kind = WeaponKind.Melee,
            Damage = 25,
            Range = 60f,
            ArcHalfAngleRadians = MathF.PI / 4f,
            Knockback = 3f,
            CooldownSeconds = 0.4f,
            BowChargeSecondsMax = 0f,
            BowBaseDamage = 0,
            BowProjectileSpeed = 0f,
            BowMaxRange = 0f,
            ChargeDamageBonusMax = 0f,
        };

        public static readonly WeaponData Axe = new()
        {
            Id = "axe",
            Kind = WeaponKind.Melee,
            Damage = 45,
            Range = 80f,
            ArcHalfAngleRadians = MathF.PI * 60f / 180f,
            Knockback = 8f,
            CooldownSeconds = 0.8f,
            BowChargeSecondsMax = 0f,
            BowBaseDamage = 0,
            BowProjectileSpeed = 0f,
            BowMaxRange = 0f,
            ChargeDamageBonusMax = 0f,
        };

        public static readonly WeaponData ShortBow = new()
        {
            Id = "bow_short",
            Kind = WeaponKind.Ranged,
            Damage = 20,
            Range = 0f,
            ArcHalfAngleRadians = 0f,
            Knockback = 0f,
            CooldownSeconds = 0.5f,
            BowChargeSecondsMax = 1.5f,
            BowBaseDamage = 20,
            BowProjectileSpeed = 350f,
            BowMaxRange = 350f,
            ChargeDamageBonusMax = 0.5f,
        };
    }
}
