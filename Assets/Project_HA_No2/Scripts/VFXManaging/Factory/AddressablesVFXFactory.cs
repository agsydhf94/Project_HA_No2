using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HA
{
    /// <summary>
    /// A VFX factory that loads visual effects using Unity Addressables.
    /// Implements the IVFXFactory interface to support asynchronous loading of effects.
    /// </summary>
    public class AddressablesVFXFactory : IVFXFactory
    {
        /// <summary>
        /// Asynchronously loads a VFX component using Addressables and invokes the callback once loaded.
        /// </summary>
        /// <param name="key">The address key of the VFX prefab to load.</param>
        /// <param name="onLoaded">Callback to receive the loaded VFX component or null on failure.</param>
        public void LoadFX(string key, Action<Component> onLoaded)
        {
            LoadAsync(key, onLoaded).Forget();
        }


        /// <summary>
        /// Internal async method that handles the Addressables loading logic.
        /// </summary>
        /// <param name="key">The address key of the VFX prefab to load.</param>
        /// <param name="onLoaded">Callback to receive the loaded VFX component or null on failure.</param>
        private async UniTask LoadAsync(string key, Action<Component> onLoaded)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(key);
                await handle.Task;

                var obj = GameObject.Instantiate(handle.Result);
                var component = obj.GetComponent<Component>();
                onLoaded?.Invoke(component);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[VFX] Failed to load effect: {key} - {ex.Message}");
                onLoaded?.Invoke(null);
            }
        }
    }
}
