using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public interface IPoolService<T> where T : MonoBehaviour
    {
        UniTask InitializeAsync(T prefab, int initialSize);
        T Get();
        void Return(T obj);
    }

    public class PoolService<T> : IPoolService<T> where T : MonoBehaviour
    {
        private readonly IObjectResolver _objectResolver;
        private readonly Queue<T> _pool = new();
        private T _prefab;
        private Transform _parent;

        public PoolService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask InitializeAsync(T prefab, int initialSize)
        {
            _prefab = prefab;
            _parent = new GameObject($"{typeof(T).Name}Pool").transform;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = _objectResolver.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }

            await UniTask.CompletedTask;
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            var newObj = _objectResolver.Instantiate(_prefab, _parent);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}