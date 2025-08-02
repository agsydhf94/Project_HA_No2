using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HA
{
    /// <summary>
    /// Implements IObjectFactory using Unity Addressables for asynchronous object loading.
    /// This factory does not support synchronous loading.
    /// </summary>
    public class AddressableObjectFactory : IObjectFactory
    {
        /// <summary>
        /// Immediate loading is not supported for addressables.
        /// This method always returns false.
        /// </summary>
        public bool TryLoadImmediate(string key, out Component component)
        {
            component = null;
            return false;
        }


        /// <summary>
        /// Loads an object asynchronously using Unity Addressables system.
        /// </summary>
        public async UniTask<Component> LoadObjectAsync(string key)
        {
            var handle = Addressables.InstantiateAsync(key);
            await handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result.GetComponent<Component>();

            Debug.LogWarning($"[Addressables] Failed to load object: {key}");
            return null;
        }
        

        /// <summary>
        /// Releases the specified Addressables instance back to the system.
        /// This method is used for proper cleanup of instantiated objects.
        /// </summary>
        public void ReturnObject(string key, Component component)
        {
            if (component != null && component.gameObject != null)
            {
                Addressables.ReleaseInstance(component.gameObject);
            }
        }
    }
}
