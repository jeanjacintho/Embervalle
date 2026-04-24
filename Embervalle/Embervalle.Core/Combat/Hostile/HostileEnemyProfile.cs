namespace Embervalle.Core.Combat.Hostile
{
    
    public readonly struct HostileEnemyProfile
    {
        public float DetectionRange { get; init; }

        public float FieldOfViewDegrees { get; init; }

        public float HearingRange { get; init; }

        public float AttackRange { get; init; }

        public int MeleeDamage { get; init; }

        public float AttackCooldownSeconds { get; init; }

        public float MoveSpeedPixelsPerSecond { get; init; }

        public bool FleeWhenLowHealth { get; init; }

        public float FleeHealthFraction { get; init; }

        public float FleeMoveSpeedScale { get; init; }

        public float ChaseLoseTimeSeconds { get; init; }

        public float SearchLookDurationSeconds { get; init; }

        public float PatrolWaitSeconds { get; init; }

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
