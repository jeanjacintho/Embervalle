namespace Embervalle.Core.Combat.Hostile
{
    /// <summary>Parâmetros de inimigo hostil (velocidade, FOV, audição, dano, tempos de IA); copiado em <see cref="CombatEnemy.HostileProfile"/>.</summary>
    public readonly struct HostileEnemyProfile
    {
        /// <summary>Raio em que o inimigo pode considerar o jogador (antes do corte por FOV).</summary>
        public float DetectionRange { get; init; }
        /// <summary>Meia abertura = metade do valor; 360 = visão omnidirecional.</summary>
        public float FieldOfViewDegrees { get; init; }
        /// <summary>Alcance de passos/ruído; reduz se o jogador não corre.</summary>
        public float HearingRange { get; init; }
        /// <summary>Distância dos pés para entrar no estado <see cref="HostileEnemyState.Attack"/>.</summary>
        public float AttackRange { get; init; }
        public int MeleeDamage { get; init; }
        public float AttackCooldownSeconds { get; init; }
        public float MoveSpeedPixelsPerSecond { get; init; }
        /// <summary>Se <c>true</c>, com HP abaixo de <see cref="FleeHealthFraction"/> tenta <see cref="HostileEnemyState.Flee"/>.</summary>
        public bool FleeWhenLowHealth { get; init; }
        public float FleeHealthFraction { get; init; }
        public float FleeMoveSpeedScale { get; init; }
        /// <summary>Sem ver nem ouvir o jogador: transição de <see cref="HostileEnemyState.Chase"/> para <see cref="HostileEnemyState.Search"/>.</summary>
        public float ChaseLoseTimeSeconds { get; init; }
        /// <summary>Tempo a olhar em círculo após chegar ao ponto de busca.</summary>
        public float SearchLookDurationSeconds { get; init; }
        /// <summary>Pausa em cada waypoint de patrulha.</summary>
        public float PatrolWaitSeconds { get; init; }

        /// <summary>Arqueiro corpo a corpo + FOV estreita; foge com pouca vida.</summary>
        public static HostileEnemyProfile Goblin =>
            new()
            {
                DetectionRange = 150f,
                FieldOfViewDegrees = 120f,
                HearingRange = 100f,
                AttackRange = 50f,
                MeleeDamage = 6,
                AttackCooldownSeconds = 0.85f,
                MoveSpeedPixelsPerSecond = 90f,
                FleeWhenLowHealth = true,
                FleeHealthFraction = 0.2f,
                FleeMoveSpeedScale = 1.2f,
                ChaseLoseTimeSeconds = 2.5f,
                SearchLookDurationSeconds = 1.4f,
                PatrolWaitSeconds = 1.2f,
            };

        /// <summary>Visão 360°; sem fuga; patrulha lenta e alcance curto.</summary>
        public static HostileEnemyProfile Slime =>
            new()
            {
                DetectionRange = 120f,
                FieldOfViewDegrees = 360f,
                HearingRange = 60f,
                AttackRange = 40f,
                MeleeDamage = 4,
                AttackCooldownSeconds = 1.1f,
                MoveSpeedPixelsPerSecond = 60f,
                FleeWhenLowHealth = false,
                FleeHealthFraction = 0.2f,
                FleeMoveSpeedScale = 1f,
                ChaseLoseTimeSeconds = 2f,
                SearchLookDurationSeconds = 1f,
                PatrolWaitSeconds = 1.5f,
            };
    }
}
