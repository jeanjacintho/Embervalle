using System;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Tipo de projétil: flecha ou projétil de feitiço.</summary>
    public enum ProjectileKind
    {
        Arrow,
        Spell,
    }

    /// <summary>Estado mutável de um projétil ativo no pool: posição, direção, velocidade e dano.</summary>
    public sealed class ProjectileState
    {
        public bool IsActive;

        public ProjectileKind Kind;

        public Vector2 Position;

        public Vector2 Direction;

        public float Speed;

        public int Damage;

        public float MaxRange;

        public float DistanceTraveled;

        public float Gravity;

        public int OwnerId;

        public SpellData? SpellData;

        /// <summary>Limpa o projétil ativo e repõe a distância percorrida.</summary>
        public void Reset()
        {
            IsActive = false;
            SpellData = null;
            DistanceTraveled = 0f;
        }
    }

    /// <summary>Pool de projéteis de tamanho fixo que reutiliza instâncias para evitar alocações durante o combate.</summary>
    public sealed class ProjectilePool
    {
        private readonly ProjectileState[] _slots;
        private readonly int _capacity;

        /// <summary>Aloca o array de <see cref="ProjectileState"/> reutilizáveis.</summary>
        public ProjectilePool(int capacity = 64)
        {
            _capacity = capacity;
            _slots = new ProjectileState[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _slots[i] = new ProjectileState();
            }
        }

        public ReadOnlySpan<ProjectileState> AllSlots => _slots.AsSpan(0, _capacity);

        /// <summary>Reutiliza o primeiro slot livre; devolve null se o pool estiver cheio.</summary>
        public ProjectileState? Spawn(
            Vector2 origin,
            Vector2 directionNormalized,
            float speed,
            int damage,
            float maxRange,
            float gravity,
            int ownerId,
            ProjectileKind kind,
            SpellData? spellData = null)
        {
            for (int i = 0; i < _capacity; i++)
            {
                ProjectileState p = _slots[i];
                if (p.IsActive)
                {
                    continue;
                }

                p.IsActive = true;
                p.Kind = kind;
                p.Position = origin;
                p.Direction = directionNormalized;
                p.Speed = speed;
                p.Damage = damage;
                p.MaxRange = maxRange;
                p.DistanceTraveled = 0f;
                p.Gravity = gravity;
                p.OwnerId = ownerId;
                p.SpellData = spellData;
                return p;
            }

            return null;
        }

        /// <summary>Reinicia o slot associado ao estado (chama <see cref="ProjectileState.Reset"/>).</summary>
        public void Deactivate(ProjectileState p)
        {
            p.Reset();
        }
    }
}
