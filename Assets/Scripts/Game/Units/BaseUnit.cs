using Managers;
using UnityEngine;

namespace Game.Units
{
    public class BaseUnit : MonoBehaviour, IDamageable
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public GridCell CurrentCell { get; set; }

        public void SetCurrentCell(GridCell cell)
        {
            CurrentCell = cell;
        }

        protected virtual void Init(int maxHealth)
        {
            MaxHealth = maxHealth > 0 ? maxHealth : 100;
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) return;

            CurrentHealth -= amount;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }
    }
}