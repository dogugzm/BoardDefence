using Configs;
using UnityEngine;

namespace Managers
{
    public interface ILevelManager
    {
        int CurrentLevel { get; }
        LevelData GetLevelData(int level);
        int GetTotalLevels();
        int StartLevel();
        int NextLevel();
        bool IsLastLevel();
        void ResetLevels();
    }

    public class LevelManager : ILevelManager
    {
        public int CurrentLevel { get; private set; }

        private LevelConfig _config;

        public LevelManager(LevelConfig config)
        {
            _config = config;
        }

        public LevelData GetLevelData(int level)
        {
            if (level < 0 || level >= _config.Levels.Count)
            {
                throw new System.ArgumentOutOfRangeException(nameof(level), "Invalid level index");
            }

            var copy = ScriptableObject.Instantiate(_config);
            return copy.Levels[level];
        }

        public int GetTotalLevels()
        {
            return _config.Levels.Count;
        }

        public int StartLevel()
        {
            CurrentLevel = 0;
            return CurrentLevel;
        }

        public int NextLevel()
        {
            if (CurrentLevel < _config.Levels.Count - 1)
            {
                CurrentLevel++;
            }

            return CurrentLevel;
        }

        public bool IsLastLevel()
        {
            return CurrentLevel >= _config.Levels.Count - 1;
        }

        public void ResetLevels()
        {
            CurrentLevel = 0;
        }
    }
}