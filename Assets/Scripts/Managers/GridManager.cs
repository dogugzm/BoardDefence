using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public interface IGridManager
    {
        UniTask InitializeAsync();
        GridCell GetCell(int x, int y);
        bool HasCell(int x, int y);
        GridCell GetRandomEnemyCell();
    }

    public class GridManager : IGridManager
    {
        private const float DefaultHeight = 1;
        private readonly Dictionary<Vector2Int, GridCell> _grid = new();
        private int _width;
        private int _height;

        private IObjectResolver _objectResolver;
        private GridCell _gridCellPrefab;
        private GridConfig _gridConfig;

        public GridManager(IObjectResolver objectResolver, GridCell gridCellPrefab, GridConfig gridConfig)
        {
            _objectResolver = objectResolver;
            _gridCellPrefab = gridCellPrefab;
            _gridConfig = gridConfig;
        }

        public async UniTask InitializeAsync()
        {
            await GenerateGridAsync(_gridConfig.Width, _gridConfig.Height);
        }

        public GridCell GetCell(int x, int y)
        {
            var key = new Vector2Int(x, y);
            return _grid.GetValueOrDefault(key);
        }

        public bool HasCell(int x, int y)
        {
            return _grid.ContainsKey(new Vector2Int(x, y));
        }

        public GridCell GetRandomEnemyCell() => GetCell(
            Random.Range(0, _gridConfig.Width),
            _gridConfig.Height - 1
        );

        private async UniTask GenerateGridAsync(int width, int height)
        {
            _grid.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var gridCell = GenerateGrid(x, y);
                    var isEnemySide = y >= _gridConfig.FriendlyGridMaxValue;
                    var cellData = new GridCell.Data(
                        new Vector2Int(x, y),
                        isEnemySide ? GridSideState.Enemy : GridSideState.Friendy,
                        GridStatusState.Free
                    );
                    gridCell.InitAsync(cellData).Forget();
                    await gridCell.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
                    _grid[new Vector2Int(x, y)] = gridCell;
                }
            }
        }

        private GridCell GenerateGrid(int x, int y)
        {
            var gridCell = _objectResolver.Instantiate(_gridCellPrefab);
            gridCell.transform.localScale = Vector3.zero;
            gridCell.gameObject.name = $"GridCell_{x}_{y}";
            float posX = x * (_gridConfig.PositionMultiplier + _gridConfig.SpaceOnX);
            float posZ = y * (_gridConfig.PositionMultiplier + _gridConfig.SpaceOnZ);
            gridCell.transform.position = new Vector3(posX, DefaultHeight, posZ);
            return gridCell;
        }
    }
}