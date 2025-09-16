using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public enum GridSideState
    {
        Enemy = 0,
        Friendy = 1
    }

    public enum GridStatusState
    {
        Free = 0,
        Occupied = 1
    }

    public class GridCell : MonoBehaviour
    {
        public class Data
        {
            public Vector2Int Coordinates { get; }
            public GridSideState SideState { get; set; }
            public GridStatusState StatusState { get; set; }

            public Data(Vector2Int coordinates, GridSideState sideState = GridSideState.Friendy,
                GridStatusState statusState = GridStatusState.Free)
            {
                Coordinates = coordinates;
                SideState = sideState;
                StatusState = statusState;
            }
        }

        public Data CellData { get; private set; }

        public bool CanDrop => (CellData.StatusState is GridStatusState.Free &&
                                CellData.SideState is GridSideState.Friendy);

        public UniTask InitAsync(Data data)
        {
            CellData = data;
            return UniTask.CompletedTask;
        }
    }
}