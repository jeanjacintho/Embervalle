using System;
using System.Collections.Generic;
using Embervalle.Core.Events;
using Embervalle.Core.Gameplay;
using Microsoft.Xna.Framework;

namespace Embervalle.Core.Combat.Hostile
{
    public static class HostileEnemyAiSystem
    {
        private const float ArrivedWaypointEps = 6f;
        private const float SearchArrivedEps = 10f;
        private const float FleeReturnRangeScale = 1.3f;

        public static void Update(
            List<CombatEnemy> enemies,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            bool playerIsRunning,
            float dt)
        {
            player.UpdateHurtInvincibility(dt);
            for (int i = 0; i < enemies.Count; i++)
            {
                CombatEnemy e = enemies[i];
                if (!e.IsHostileAi || e.Health.Current <= 0)
                {
                    continue;
                }

                TickOne(e, player, viewportWidth, viewportHeight, playerIsRunning, dt);
            }
        }

        private static void TickOne(
            CombatEnemy e,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            bool playerIsRunning,
            float dt)
        {
            HostileEnemyProfile p = e.HostileProfile;
            e.AttackCooldownRemaining = MathF.Max(0f, e.AttackCooldownRemaining - dt);

            bool los = EnemyPerception.HasLineOfSightSimple(e.FeetPosition, player.FeetPosition);
            bool seen = EnemyPerception.IsPlayerVisible(
                e.FeetPosition,
                e.FacingDirection,
                player.FeetPosition,
                p,
                los);
            bool heard = EnemyPerception.IsPlayerHeard(e.FeetPosition, player.FeetPosition, p, playerIsRunning);
            if (seen || heard)
            {
                e.LastKnownPlayerPosition = player.FeetPosition;
            }

            bool lowHpFlee = p.FleeWhenLowHealth
                && (float)e.Health.Current <= e.Health.Max * p.FleeHealthFraction
                && e.HostileState is HostileEnemyState.Chase
                    or HostileEnemyState.Attack
                    or HostileEnemyState.Alert;

            if (lowHpFlee)
            {
                e.HostileState = HostileEnemyState.Flee;
            }

            switch (e.HostileState)
            {
                case HostileEnemyState.Patrol:
                    UpdatePatrol(e, p, seen, heard, player, viewportWidth, viewportHeight, dt);
                    break;
                case HostileEnemyState.Alert:
                    UpdateAlert(e, p, seen, player, viewportWidth, viewportHeight, dt);
                    break;
                case HostileEnemyState.Chase:
                    UpdateChase(
                        e,
                        p,
                        seen,
                        heard,
                        player,
                        viewportWidth,
                        viewportHeight,
                        dt);
                    break;
                case HostileEnemyState.Attack:
                    UpdateAttack(e, p, seen, player, viewportWidth, viewportHeight, dt);
                    break;
                case HostileEnemyState.Search:
                    UpdateSearch(e, p, seen, player, viewportWidth, viewportHeight, dt);
                    break;
                case HostileEnemyState.Flee:
                    UpdateFlee(e, p, player, viewportWidth, viewportHeight, dt);
                    break;
            }
        }

        private static void UpdatePatrol(
            CombatEnemy e,
            in HostileEnemyProfile p,
            bool seen,
            bool heard,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            if (seen)
            {
                e.HostileState = HostileEnemyState.Chase;
                e.EngageLoseTimer = 0f;
                SetFacingToward(e, player.FeetPosition);
                return;
            }

            if (heard)
            {
                e.LastKnownPlayerPosition = player.FeetPosition;
                e.HostileState = HostileEnemyState.Alert;
                e.AlertTimer = 0.45f;
                SetFacingToward(e, player.FeetPosition);
                return;
            }

            if (e.PatrolWorldWaypoints.Length < 1)
            {
                return;
            }

            if (e.PatrolWaitRemaining > 0f)
            {
                e.PatrolWaitRemaining = MathF.Max(0f, e.PatrolWaitRemaining - dt);
                return;
            }

            Vector2 target = e.PatrolWorldWaypoints[e.PatrolWaypointIndex];
            Vector2 d = target - e.FeetPosition;
            if (d.LengthSquared() < ArrivedWaypointEps * ArrivedWaypointEps)
            {
                e.PatrolWaypointIndex = (e.PatrolWaypointIndex + 1) % e.PatrolWorldWaypoints.Length;
                e.PatrolWaitRemaining = p.PatrolWaitSeconds;
                return;
            }

            MoveAlongDelta(e, d, p.MoveSpeedPixelsPerSecond, dt, viewportWidth, viewportHeight, updateFacing: true);
        }

        private static void UpdateAlert(
            CombatEnemy e,
            in HostileEnemyProfile p,
            bool seen,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            e.AlertTimer -= dt;
            if (seen)
            {
                e.HostileState = HostileEnemyState.Chase;
                e.EngageLoseTimer = 0f;
                SetFacingToward(e, player.FeetPosition);
                return;
            }

            if (e.LastKnownPlayerPosition is not { } pk)
            {
                e.HostileState = HostileEnemyState.Patrol;
                return;
            }

            Vector2 d = pk - e.FeetPosition;
            if (d.LengthSquared() < 20f * 20f)
            {
                e.HostileState = HostileEnemyState.Search;
                e.SearchPhaseTimer = 0f;
                e.FacingDirection = d.LengthSquared() > 0.1f
                    ? Vector2.Normalize(d)
                    : e.FacingDirection;
                return;
            }

            float speed = p.MoveSpeedPixelsPerSecond * 0.9f;
            MoveAlongDelta(e, d, speed, dt, viewportWidth, viewportHeight, updateFacing: true);
            if (e.AlertTimer <= 0f)
            {
                e.HostileState = HostileEnemyState.Search;
            }
        }

        private static void UpdateChase(
            CombatEnemy e,
            in HostileEnemyProfile p,
            bool seen,
            bool heard,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            if (seen || heard)
            {
                e.EngageLoseTimer = 0f;
            }
            else
            {
                e.EngageLoseTimer += dt;
            }

            if (e.EngageLoseTimer >= p.ChaseLoseTimeSeconds)
            {
                e.HostileState = HostileEnemyState.Search;
                e.SearchPhaseTimer = 0f;
                e.FacingDirection = e.LastKnownPlayerPosition is { } lk
                    ? SafeNormalize(lk - e.FeetPosition)
                    : e.FacingDirection;
                return;
            }

            e.FacingDirection = SafeNormalize(player.FeetPosition - e.FeetPosition);
            float dist = Vector2.Distance(e.FeetPosition, player.FeetPosition);
            if (dist <= p.AttackRange
                && (seen || dist < p.AttackRange * 0.5f)
                && InMeleeAngle(e, player))
            {
                e.HostileState = HostileEnemyState.Attack;
                return;
            }

            Vector2 d = player.FeetPosition - e.FeetPosition;
            MoveAlongDelta(e, d, p.MoveSpeedPixelsPerSecond, dt, viewportWidth, viewportHeight, updateFacing: false);
        }

        private static void UpdateAttack(
            CombatEnemy e,
            in HostileEnemyProfile p,
            bool seen,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            e.FacingDirection = SafeNormalize(player.FeetPosition - e.FeetPosition);
            float dist = Vector2.Distance(e.FeetPosition, player.FeetPosition);
            if (dist > p.AttackRange + 4f
                || (!seen && dist > p.AttackRange * 0.4f)
                || !InMeleeAngle(e, player))
            {
                e.HostileState = HostileEnemyState.Chase;
                e.EngageLoseTimer = 0f;
                return;
            }

            if (e.AttackCooldownRemaining <= 0f)
            {
                if (player.TryApplyHurt(p.MeleeDamage, 0.45f))
                {
                    EventBus.Publish(
                        new DamageTakenEvent
                        {
                            Amount = p.MeleeDamage,
                            TargetId = 0,
                        });
                }

                e.AttackCooldownRemaining = p.AttackCooldownSeconds;
            }
        }

        private static void UpdateSearch(
            CombatEnemy e,
            in HostileEnemyProfile p,
            bool seen,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            if (seen)
            {
                e.HostileState = HostileEnemyState.Chase;
                e.EngageLoseTimer = 0f;
                return;
            }

            if (e.LastKnownPlayerPosition is not { } goal)
            {
                e.HostileState = HostileEnemyState.Patrol;
                return;
            }

            Vector2 d = goal - e.FeetPosition;
            if (d.LengthSquared() < SearchArrivedEps * SearchArrivedEps)
            {
                e.SearchPhaseTimer += dt;
                float t = e.SearchPhaseTimer;
                e.FacingDirection = new Vector2(MathF.Cos(t * 2.2f), MathF.Sin(t * 2.2f));
                if (e.SearchPhaseTimer >= p.SearchLookDurationSeconds)
                {
                    e.HostileState = HostileEnemyState.Patrol;
                    e.EngageLoseTimer = 0f;
                }
            }
            else
            {
                MoveAlongDelta(
                    e,
                    d,
                    p.MoveSpeedPixelsPerSecond * 0.8f,
                    dt,
                    viewportWidth,
                    viewportHeight,
                    updateFacing: true);
            }
        }

        private static void UpdateFlee(
            CombatEnemy e,
            in HostileEnemyProfile p,
            PlayerBody player,
            int viewportWidth,
            int viewportHeight,
            float dt)
        {
            if (!p.FleeWhenLowHealth)
            {
                e.HostileState = HostileEnemyState.Chase;
                return;
            }

            if ((float)e.Health.Current > e.Health.Max * p.FleeHealthFraction * 1.1f)
            {
                e.HostileState = HostileEnemyState.Patrol;
                return;
            }

            float fleeSpeed = p.MoveSpeedPixelsPerSecond * p.FleeMoveSpeedScale;
            Vector2 away = e.FeetPosition - player.FeetPosition;
            if (away.LengthSquared() < 0.1f)
            {
                away = -e.FacingDirection;
            }
            else
            {
                away = Vector2.Normalize(away);
            }

            e.FacingDirection = away;
            e.FeetPosition += away * fleeSpeed * dt;
            CombatEnemyAiShared.ClampFeetToViewport(e, viewportWidth, viewportHeight);
            if (Vector2.Distance(e.FeetPosition, player.FeetPosition) >= p.DetectionRange * FleeReturnRangeScale)
            {
                e.HostileState = HostileEnemyState.Patrol;
            }
        }

        private static bool InMeleeAngle(CombatEnemy e, PlayerBody player)
        {
            Vector2 d = player.FeetPosition - e.FeetPosition;
            if (d.LengthSquared() < 0.1f)
            {
                return true;
            }

            d = Vector2.Normalize(d);
            return EnemyPerception.AngleBetweenRadians(e.FacingDirection, d) <= MathHelper.ToRadians(55f);
        }

        private static void MoveAlongDelta(
            CombatEnemy e,
            Vector2 worldDelta,
            float speed,
            float dt,
            int viewportWidth,
            int viewportHeight,
            bool updateFacing)
        {
            if (worldDelta.LengthSquared() < 0.1f)
            {
                return;
            }

            Vector2 dir = Vector2.Normalize(worldDelta);
            if (updateFacing)
            {
                e.FacingDirection = dir;
            }

            e.FeetPosition += dir * speed * dt;
            CombatEnemyAiShared.ClampFeetToViewport(e, viewportWidth, viewportHeight);
        }

        private static void SetFacingToward(CombatEnemy e, Vector2 target)
        {
            e.FacingDirection = SafeNormalize(target - e.FeetPosition);
        }

        private static Vector2 SafeNormalize(Vector2 v)
        {
            if (v.LengthSquared() < 1e-4f)
            {
                return Vector2.UnitY;
            }

            return Vector2.Normalize(v);
        }
    }
}
