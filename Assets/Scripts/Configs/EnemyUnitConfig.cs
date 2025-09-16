using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyUnitConfig", menuName = "Configs/EnemyUnitConfig")]
    public class EnemyUnitConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float SecondPerGrid { get; private set; }
    }
}