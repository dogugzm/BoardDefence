using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using Game.Units;
using UnityEngine;

namespace Managers
{
    public class EnemyWaveManager
    {
        private readonly IPoolService<EnemyUnit> _enemyPool;
        private readonly ILevelManager _levelManager;
        private readonly IGridManager _gridManager;

        private List<EnemyUnitEntry> _enemyEntries;

        public EnemyWaveManager(IPoolService<EnemyUnit> enemyPool, ILevelManager levelManager, IGridManager gridManager)
        {
            _enemyPool = enemyPool;
            _levelManager = levelManager;
            _gridManager = gridManager;
        }

        public void GenerateEnemies(List<EnemyUnitEntry> enemyEntries)
        {
            _enemyEntries = enemyEntries;
            SpawnWave().Forget();
        }

        private async UniTask SpawnWave()
        {
            if (_enemyEntries == null || _enemyEntries.Count == 0)
            {
                Debug.Log("No enemies to spawn for this level!");
                return;
            }

            foreach (var enemyEntry in _enemyEntries)
            {
                for (int i = 0; i < enemyEntry.Count; i++)
                {
                    var enemy = _enemyPool.Get();
                    var randomCell = _gridManager.GetRandomEnemyCell();
                    enemy.SetCurrentCell(randomCell);
                    enemy.transform.position =
                        new Vector3(randomCell.transform.position.x, 3, randomCell.transform.position.z);
                    enemy.SetData(enemyEntry.Config);
                    await UniTask.WaitForSeconds(2f);
                }
            }
        }
    }
}