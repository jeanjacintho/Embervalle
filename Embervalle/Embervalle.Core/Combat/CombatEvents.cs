using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    /// <summary>Evento publicado quando um alvo recebe dano.</summary>
    public sealed class DamageTakenEvent
    {
        public required int Amount { get; init; }

        public int? TargetId { get; init; }
    }

    /// <summary>Evento publicado quando um feitiço é lançado com sucesso.</summary>
    public sealed class SpellCastEvent
    {
        public required SpellData Spell { get; init; }
    }

    /// <summary>Evento publicado quando uma flecha atinge o alcance máximo e é removida do pool.</summary>
    public sealed class ArrowExpiredEvent
    {
        public int PoolIndex { get; init; }
    }

    /// <summary>Evento publicado quando um efeito de estado é aplicado a um personagem.</summary>
    public sealed class StatusAppliedEvent
    {
        public StatusEffect Effect { get; init; }
    }
}
