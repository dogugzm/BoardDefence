using System.Collections.Generic;
using Configs;
using Game;
using Game.Units;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("PREFABS")] [SerializeField] private GridCell _gridCellPrefab;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private DefenceUnit _defenceUnit;
        [SerializeField] private EnemyUnit _enemyUnit;
        [SerializeField] private DefenceUnitView _defenceUnitViewPrefab;

        [Header("CONFIGS")] [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private List<DefenceUnitConfig> _defenceUnitConfigs;
        [SerializeField] private List<EnemyUnitConfig> _enemyUnitConfigs;
        [SerializeField] private LevelConfig _levelConfig;

        [SerializeField] private Transform defenceUnitViewHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GridManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameInitializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<TargetManager>(Lifetime.Singleton).As<ITargetManager>();
            builder.Register<DragAndDropService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<DropManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelManager>(Lifetime.Singleton).As<ILevelManager>();
            builder.Register<UIManager>(Lifetime.Singleton)
                .WithParameter("defenceUnitViewHolder", defenceUnitViewHolder)
                .WithParameter("defenceUnitViewPrefab", _defenceUnitViewPrefab)
                .AsSelf();

            builder.Register<EnemyWaveManager>(Lifetime.Singleton).AsSelf();

            builder.Register(typeof(PoolService<>), Lifetime.Singleton).As(typeof(IPoolService<>));

            builder.RegisterComponent(_gridCellPrefab);
            builder.RegisterComponent(_projectilePrefab);
            builder.RegisterComponent(_defenceUnit);
            builder.RegisterComponent(_enemyUnit);


            builder.RegisterComponent(_gridConfig);
            builder.RegisterComponent(_levelConfig);
            builder.RegisterInstance(_defenceUnitConfigs);
            builder.RegisterInstance(_enemyUnitConfigs);

            builder.RegisterComponentInHierarchy<Camera>();
            builder.RegisterComponentInHierarchy<GraphicRaycaster>();
        }
    }
}