using Cysharp.Threading.Tasks;
using Game;
using Game.Units;
using UnityEngine;
using VContainer.Unity;

namespace Managers
{
    public class GameInitializer : IStartable
    {
        private readonly IGridManager _gridManager;
        private readonly IPoolService<Projectile> _projectilePoolService;
        private readonly Projectile _projectilePrefab;
        private readonly ILevelManager _levelService;
        private readonly UIManager _uiManager;
        private readonly EnemyWaveManager _enemyWaveManager;
        private readonly IPoolService<EnemyUnit> _enemyPoolService;
        private readonly EnemyUnit _enemyUnitPrefab;

        public GameInitializer(IGridManager gridManager, IPoolService<Projectile> projectilePoolService,
            Projectile projectilePrefab,
            ILevelManager levelService, UIManager uiManager, EnemyWaveManager enemyWaveManager,
            IPoolService<EnemyUnit> enemyPoolService, EnemyUnit enemyUnitPrefab)
        {
            _gridManager = gridManager;
            _projectilePoolService = projectilePoolService;
            _projectilePrefab = projectilePrefab;
            _levelService = levelService;
            _uiManager = uiManager;
            _enemyWaveManager = enemyWaveManager;
            _enemyPoolService = enemyPoolService;
            _enemyUnitPrefab = enemyUnitPrefab;
        }

        public async void Start()
        {
            await _gridManager.InitializeAsync();
            _projectilePoolService.InitializeAsync(_projectilePrefab, 10).Forget();
            _enemyPoolService.InitializeAsync(_enemyUnitPrefab, 20).Forget();

            var currentLevel = _levelService.StartLevel();
            var currentLevelData = _levelService.GetLevelData(currentLevel);

            _uiManager.GenerateDefenceUnitViews(currentLevelData.DefenceUnits);
            _enemyWaveManager.GenerateEnemies(currentLevelData.EnemyUnits);
        }
    }
}