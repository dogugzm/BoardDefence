using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using UnityEngine;
using VContainer;

namespace Game.Units
{
    public interface IMovable
    {
        float SecondPerGrid { get; }
        UniTask Move();
    }

    public class EnemyUnit : BaseUnit, IMovable
    {
        public float SecondPerGrid { get; private set; }
        private EnemyUnitConfig _config;
        private IGridManager _gridManager;
        private ITargetManager _targetManager;

        [Inject]
        private void Construct(IGridManager gridManager, ITargetManager targetManager)
        {
            _gridManager = gridManager;
            _targetManager = targetManager;
        }

        public void SetData(EnemyUnitConfig config)
        {
            _config = config;
            Init(_config.MaxHealth);
            SecondPerGrid = _config.SecondPerGrid;
        }

        private void OnEnable()
        {
            _targetManager.AddTarget(this);
        }

        private void OnDisable()
        {
            _targetManager.RemoveTarget(this);
        }

        private void Start()
        {
            Move().Forget();
        }

        public async UniTask Move()
        {
            while (IsAlive)
            {
                if (CurrentCell == null)
                {
                    Debug.LogError("CurrentCell is not set on EnemyUnit.");
                    return;
                }

                var nextX = CurrentCell.CellData.Coordinates.x;
                var nextY = CurrentCell.CellData.Coordinates.y - 1;

                if (_gridManager.HasCell(nextX, nextY))
                {
                    var nextCell = _gridManager.GetCell(nextX, nextY);
                    CurrentCell = nextCell;

                    await transform.DOMoveZ(nextCell.transform.position.z, 0.5f).SetEase(Ease.OutBack);

                    await UniTask.Delay((int)(SecondPerGrid * 1000));
                }
                else
                {
                    Debug.Log("Enemy reached the end of the grid.");
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}