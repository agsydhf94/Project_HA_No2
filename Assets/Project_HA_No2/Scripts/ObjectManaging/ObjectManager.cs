using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines the source type for object creation: either from an Object Pool or Addressables.
    /// </summary>    
    public enum ObjectSourceType
    {
        ObjectPooling,
        Addressables
    }
    
    /// <summary>
    /// Central manager responsible for spawning and returning game objects using either pooling or addressables.
    /// Abstracts the loading strategy behind a unified interface.
    /// </summary>
    public class ObjectManager : SingletonBase<ObjectManager>, IObjectSpawner, IObjectReturn
    {
        private IObjectFactory poolFactory;
        private IObjectFactory addressablesFactory;

        /// <summary>
        /// Event triggered when an object is returned to the system.
        /// </summary>
        public event Action OnReturnRequested;


        /// <summary>
        /// Initializes the manager by setting up both pooling and addressables factories.
        /// </summary>
        public override void Awake()
        {
            poolFactory = new PoolObjectFactory(ObjectPool.Instance);
            addressablesFactory = new AddressableObjectFactory();
        }


        /// <summary>
        /// Attempts to spawn an object synchronously from the selected source.
        /// If synchronous loading fails, triggers fallback asynchronous loading in the background.
        /// </summary>
        /// <param name="key">The unique identifier of the object to load.</param>
        /// <param name="position">The target world position for the object.</param>
        /// <param name="rotation">The desired rotation of the object.</param>
        /// <param name="parent">Optional parent transform.</param>
        /// <param name="sourceType">Specifies whether to use pooling or addressables.</param>
        /// <returns>The spawned Component if loaded immediately, otherwise null (asynchronous).</returns>
        public Component Spawn(string key, Vector3 position, Quaternion rotation, Transform parent, ObjectSourceType sourceType)
        {
            IObjectFactory factory = GetFactory(sourceType);

            if (factory.TryLoadImmediate(key, out var component))
            {
                Setup(component.gameObject, position, rotation, parent);
                return component;
            }

            // Async fallback - object will be loaded and set up later
            _ = LoadAsyncAndSetup(factory, key, position, rotation, parent);
            return null;
        }


        /// <summary>
        /// Loads an object asynchronously and sets it up once available.
        /// </summary>
        private async UniTaskVoid LoadAsyncAndSetup(IObjectFactory factory, string key, Vector3 pos, Quaternion rot, Transform parent)
        {
            var component = await factory.LoadObjectAsync(key);
            if (component != null)
                Setup(component.gameObject, pos, rot, parent);
        }

        /// <summary>
        /// Applies transform and activation to the loaded object.
        /// </summary>
        private void Setup(GameObject obj, Vector3 pos, Quaternion rot, Transform parent)
        {
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
        }


        /// <summary>
        /// Returns an object to the pool using its key.
        /// </summary>
        public void Return(string key, Component component)
        {
            ObjectPool.Instance.ReturnToPool(key, component);
        }


        /// <summary>
        /// Chooses the correct factory based on the requested source type.
        /// </summary>
        private IObjectFactory GetFactory(ObjectSourceType type)
        {
            return type == ObjectSourceType.Addressables ? addressablesFactory : poolFactory;
        }
    }
}
