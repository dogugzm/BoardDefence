using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    interface IAttackable
    {
        Transform transform { get; }
        UniTask Attack(IDamageable target);
        int AttackPower { get; }
        int AttackRange { get; }
        float AttackInterval { get; }
        Direction AttackDirection { get; }
    }
}