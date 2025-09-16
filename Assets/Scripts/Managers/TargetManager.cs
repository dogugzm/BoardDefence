using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Units;
using UnityEngine;

namespace Managers
{
    public interface ITargetManager
    {
        void AddTarget(IDamageable target);
        void RemoveTarget(IDamageable target);
        IDamageable GetTargetInRange(BaseUnit unit, int range, Direction attackDirection);
    }

    public class TargetManager : ITargetManager
    {
        private readonly List<IDamageable> _targets = new();

        public void AddTarget(IDamageable target)
        {
            if (!_targets.Contains(target))
            {
                _targets.Add(target);
            }
        }

        public void RemoveTarget(IDamageable target)
        {
            _targets.Remove(target);
        }

        public IDamageable GetTargetInRange(BaseUnit unit, int range, Direction attackDirection)
        {
            if (unit.CurrentCell == null) return null;

            var validTargets = _targets
                .Where(IsValidTarget)
                .Where(t => IsInAttackDirection(unit, t, attackDirection))
                .ToList();

            return GetClosestTarget(unit, validTargets, range);
        }

        private bool IsValidTarget(IDamageable target)
        {
            if (target is not BaseUnit targetUnit || !target.IsAlive || targetUnit.CurrentCell == null)
            {
                if (target is { IsAlive: false })
                    RemoveTarget(target);

                return false;
            }

            return true;
        }

        private bool IsInAttackDirection(BaseUnit unit, IDamageable target, Direction attackDirection)
        {
            if (attackDirection != Direction.Forward) return true;

            var targetUnit = (BaseUnit)target;
            return targetUnit.CurrentCell.CellData.Coordinates.x == unit.CurrentCell.CellData.Coordinates.x;
        }

        private IDamageable GetClosestTarget(BaseUnit unit, IEnumerable<IDamageable> targets, int range)
        {
            IDamageable closestTarget = null;
            int closestDistance = int.MaxValue;

            foreach (var target in targets)
            {
                var targetUnit = (BaseUnit)target;
                int distance = Mathf.Abs(unit.CurrentCell.CellData.Coordinates.x -
                                         targetUnit.CurrentCell.CellData.Coordinates.x) +
                               Mathf.Abs(unit.CurrentCell.CellData.Coordinates.y -
                                         targetUnit.CurrentCell.CellData.Coordinates.y);

                if (distance <= range && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }
    }
}