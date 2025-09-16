using UnityEngine;

namespace Game
{
    public interface IDamageable
    {
        Transform transform { get; }
        void TakeDamage(int amount);
        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsAlive { get; }
    }
}