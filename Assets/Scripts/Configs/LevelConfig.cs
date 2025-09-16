using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }

    [System.Serializable]
    public class DefenceUnitEntry
    {
        public DefenceUnitConfig Config;
        public int Count;
    }

    [System.Serializable]
    public class EnemyUnitEntry
    {
        public EnemyUnitConfig Config;
        public int Count;
    }

    [System.Serializable]
    public class LevelData
    {
        public List<DefenceUnitEntry> DefenceUnits;
        public List<EnemyUnitEntry> EnemyUnits;
    }
}