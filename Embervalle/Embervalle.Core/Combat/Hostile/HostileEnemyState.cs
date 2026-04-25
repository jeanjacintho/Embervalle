namespace Embervalle.Core.Combat.Hostile
{
    /// <summary>Estados da FSM de inimigos: patrulha, alerta, perseguição, ataque, busca, fuga.</summary>
    public enum HostileEnemyState
    {
        /// <summary>Persegue waypoints; não combate.</summary>
        Patrol,
        /// <summary>Ouviu o jogador e aproxima-se da última posição (barulho não confirmado visualmente).</summary>
        Alert,
        /// <summary>Persegue o jogador; perde o alvo após <see cref="HostileEnemyProfile.ChaseLoseTimeSeconds"/>.</summary>
        Chase,
        /// <summary>À distância de ataque: golpes corpo a corpo com cooldown.</summary>
        Attack,
        /// <summary>Vai à última posição vista e "procura" antes de voltar à patrulha.</summary>
        Search,
        /// <summary>Abandona o combate e afasta-se (se o perfil permitir fuga com HP baixo).</summary>
        Flee,
    }
}
