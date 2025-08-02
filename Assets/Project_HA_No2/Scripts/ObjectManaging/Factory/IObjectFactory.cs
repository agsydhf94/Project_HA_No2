using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines a factory interface for object creation with both synchronous (pooling) and asynchronous (addressables) support.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Attempts to synchronously load an object from a fast-access source like an object pool.
        /// If successful, returns true and outputs the component instance.
        /// </summary>
        /// <param name="key">The identifier of the object to load.</param>
        /// <param name="component">The loaded component instance (if successful).</param>
        /// <returns>True if the object was loaded immediately (e.g., from pool); otherwise, false.</returns>
        bool TryLoadImmediate(string key, out Component component);

        /// <summary>
        /// Loads an object asynchronously from a slower source such as Addressables.
        /// This is the fallback path if immediate loading fails.
        /// </summary>
        /// <param name="key">The identifier of the object to load.</param>
        /// <returns>A UniTask that completes with the loaded component.</returns>
        UniTask<Component> LoadObjectAsync(string key);


        /// <summary>
        /// Returns or releases the specified object based on the implementation's source (e.g., pool, addressables).
        /// This ensures proper cleanup or reuse depending on the factory type.
        /// </summary>
        /// <param name="key">The identifier of the object to return or release.</param>
        /// <param name="component">The component instance to return or release.</param>
        void ReturnObject(string key, Component component);
    }
}
