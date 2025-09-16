using Game;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "DefenceUnitConfig", menuName = "Configs/DefenceUnitConfig")]
    public class DefenceUnitConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int AttackPower { get; private set; }
        [field: SerializeField] public int AttackRange { get; private set; }
        [field: SerializeField] public float AttackInterval { get; private set; }
        [field: SerializeField] public Direction AttackDirection { get; private set; }
    }
}