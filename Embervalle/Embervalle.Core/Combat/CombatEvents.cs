using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat
{
    public sealed class DamageTakenEvent
    {
        public required int Amount { get; init; }

        public int? TargetId { get; init; }
    }

    public sealed class SpellCastEvent
    {
        public required SpellData Spell { get; init; }
    }

    public sealed class ArrowExpiredEvent
    {
        public int PoolIndex { get; init; }
    }

    public sealed class StatusAppliedEvent
    {
        public StatusEffect Effect { get; init; }
    }
}
