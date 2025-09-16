using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using Managers;
using VContainer;

namespace Game.Units
{
    public class DefenceUnit : BaseUnit, IAttackable
    {
        public int AttackPower { get; private set; }
        public int AttackRange { get; private set; }
        public float AttackInterval { get; private set; }
        public Direction AttackDirection { get; private set; }

        private CancellationTokenSource attackCts;
        private DefenceUnitConfig _config;
        private IPoolService<Projectile> _projectilePool;
        private ITargetManager _targetManager;

        [Inject]
        public virtual void Construct(IPoolService<Projectile> projectilePool,
            ITargetManager targetManager)
        {
            _projectilePool = projectilePool;
            _targetManager = targetManager;
        }

        public void SetData(DefenceUnitConfig config)
        {
            _config = config;
            Init(_config.MaxHealth);
            attackCts = new CancellationTokenSource();
            CheckAttacking().Forget();
        }

        private async UniTask CheckAttacking()
        {
            while (attackCts is not null && !attackCts.IsCancellationRequested)
            {
                var target = _targetManager.GetTargetInRange(this, AttackRange,AttackDirection);
                if (target != null)
                {
                    Attack(target).Forget();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(AttackInterval),
                    cancellationToken: attackCts.Token);
            }
        }

        protected override void Init(int maxHealth)
        {
            base.Init(maxHealth);
            AttackPower = _config.AttackPower;
            AttackRange = _config.AttackRange;
            AttackInterval = _config.AttackInterval;
            AttackDirection = _config.AttackDirection;
        }

        public UniTask Attack(IDamageable target)
        {
            var projectile = _projectilePool.Get();
            projectile.transform.position = transform.position;
            projectile.Initialize(target, AttackPower, _projectilePool);
            return UniTask.CompletedTask;
        }
    }
}