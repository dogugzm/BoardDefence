using System.Collections.Generic;
using Configs;
using Game;
using Game.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public interface IDropManager
    {
        bool TryHandleDrop(Vector3 screenPosition, DefenceUnitConfig defenceUnitConfig);
    }

    public class DropManager : IDropManager
    {
        private const float DROP_HEIGHT = 3;
        private readonly IObjectResolver _objectResolver;
        private readonly DefenceUnit _defenceUnit;
        private readonly List<DefenceUnitConfig> _defenceUnitConfigs;
        private readonly Camera _camera;

        public DropManager(IObjectResolver objectResolver, DefenceUnit defenceUnit,
            List<DefenceUnitConfig> defenceUnitConfigs, Camera camera)
        {
            _objectResolver = objectResolver;
            _defenceUnit = defenceUnit;
            _defenceUnitConfigs = defenceUnitConfigs;
            _camera = camera;
        }

        public bool TryHandleDrop(Vector3 screenPosition, DefenceUnitConfig defenceUnitConfig)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit)) return false;
            if (!hit.collider.TryGetComponent<GridCell>(out var gridCell)) return false;
            if (!gridCell.CanDrop) return false;

            var unit = _objectResolver.Instantiate(_defenceUnit);
            unit.SetData(defenceUnitConfig);
            unit.SetCurrentCell(gridCell);
            unit.transform.position =
                new Vector3(gridCell.transform.position.x, DROP_HEIGHT, gridCell.transform.position.z);

            return true;
        }
    }
}