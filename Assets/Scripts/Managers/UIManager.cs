using System.Collections.Generic;
using Configs;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIManager
    {
        private DefenceUnitView _defenceUnitViewPrefab;
        private Transform _defenceUnitHolder;

        private Dictionary<DefenceUnitEntry, DefenceUnitView> _defenceUnitViews;

        public UIManager(Transform defenceUnitViewHolder, DefenceUnitView defenceUnitViewPrefab)
        {
            _defenceUnitViewPrefab = defenceUnitViewPrefab;
            _defenceUnitHolder = defenceUnitViewHolder;
        }

        public void GenerateDefenceUnitViews(List<DefenceUnitEntry> defenceUnitEntries)
        {
            _defenceUnitViews = new();
            foreach (var entry in defenceUnitEntries)
            {
                var view = Object.Instantiate(_defenceUnitViewPrefab, _defenceUnitHolder);
                var data = new DefenceUnitView.Data(entry);
                view.InitAsync(data);
                _defenceUnitViews.Add(entry, view);
            }
        }
    }
}