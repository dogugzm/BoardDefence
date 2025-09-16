using Managers;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private const float SPEED_MULTIPLIER = 40f;
        private int _damage;
        private IDamageable _target;
        private IPoolService<Projectile> _pool;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(IDamageable target, int damage, IPoolService<Projectile> pool)
        {
            _target = target;
            _damage = damage;
            _pool = pool;
            // Look at the target
            if (_target is MonoBehaviour targetMonoBehaviour)
            {
                transform.LookAt(targetMonoBehaviour.transform);
            }
        }

        void Update()
        {
            if (_target is { IsAlive: true })
            {
                if (_target is MonoBehaviour targetMonoBehaviour)
                {
                    Vector3 direction = (targetMonoBehaviour.transform.position - transform.position).normalized;
                    _rb.velocity = direction * SPEED_MULTIPLIER;
                }
            }
            else
            {
                // If target is dead or null, return to pool
                _pool.Return(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable == _target)
                {
                    damageable.TakeDamage(_damage);
                    _pool.Return(this);
                }
            }
        }
    }
}