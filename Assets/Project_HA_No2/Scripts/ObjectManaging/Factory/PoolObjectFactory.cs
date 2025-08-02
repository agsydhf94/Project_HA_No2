using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Implements IObjectFactory using an object pool to provide fast, reusable object instantiation.
    /// This factory does not support asynchronous loading.
    /// </summary>
    public class PoolObjectFactory : IObjectFactory
    {
        private readonly ObjectPool pool;

        public PoolObjectFactory(ObjectPool pool)
        {
            this.pool = pool;
        }


        /// <summary>
        /// Attempts to synchronously load an object from the object pool.
        /// </summary>
        public bool TryLoadImmediate(string key, out Component component)
        {
            component = pool.GetFromPool<Component>(key);
            return component != null;
        }


        /// <summary>
        /// Asynchronous loading is not supported for pooled objects.
        /// This method always returns null.
        /// </summary>
        public UniTask<Component> LoadObjectAsync(string key)
        {
            // Pool에서는 FallbackPath 필요 없음, 무조건 실패 처리
            return UniTask.FromResult<Component>(null);
        }


        /// <summary>
        /// Returns the specified component back to the object pool for future reuse.
        /// </summary>
        /// <param name="key">The identifier used to categorize the pooled object.</param>
        /// <param name="component">The component instance to return to the pool.</param>
        public void ReturnObject(string key, Component component)
        {
            pool.ReturnToPool(key, component);
        }
    }
}
