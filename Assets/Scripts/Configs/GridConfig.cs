using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Configs/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
        [field: SerializeField] public int PositionMultiplier { get; private set; }
        [field: SerializeField] public int SpaceOnX { get; private set; }
        [field: SerializeField] public int SpaceOnZ { get; private set; }
        [field: SerializeField] public int FriendlyGridMaxValue { get; private set; }
    }
}