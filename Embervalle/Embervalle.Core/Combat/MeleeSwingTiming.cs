namespace Embervalle.Core.Combat
{
    /// <summary>Constantes de temporização da animação de golpe corpo a corpo: duração de frame e de acompanhamento.</summary>
    public static class MeleeSwingTiming
    {
        /// <summary>Duração de cada frame do arco de melée (segundos).</summary>
        public const float FrameDurationSeconds = 0.05f;

        /// <summary>Tempo extra após o último frame antes de libertar o bloqueio de movimento.</summary>
        public const float FollowThroughSeconds = 0.07f;

        /// <summary>Máximo de sub-passos de animação a aplicar num único frame para acompanhar o relógio.</summary>
        public const int MaxSwingCatchUpStepsPerFrame = 8;
    }
}
