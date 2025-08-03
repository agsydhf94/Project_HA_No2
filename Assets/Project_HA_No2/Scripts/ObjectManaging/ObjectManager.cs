using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines the source type for object creation: either from an Object Pool or Addressables.
    /// </summary>    
    public enum ObjectSourceType
    {
        ObjectPooling,
        UIPooling,
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

        private readonly Dictionary<string, ObjectSourceType> keyToSourceType = new();


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
            keyToSourceType[key] = sourceType;
            IObjectFactory factory = GetFactory(sourceType);

            if (factory.TryLoadImmediate(key, out var component))
            {
                Setup(component.gameObject, position, rotation, parent, sourceType);
                return component;
            }

            // Async fallback - object will be loaded and set up later
            _ = LoadAsyncAndSetup(factory, key, position, rotation, parent, sourceType);
            return null;
        }


        /// <summary>
        /// Loads an object asynchronously and sets it up once available.
        /// </summary>
        private async UniTask LoadAsyncAndSetup(IObjectFactory factory, string key, Vector3 pos, Quaternion rot, Transform parent, ObjectSourceType sourceType)
        {
            var component = await factory.LoadObjectAsync(key);
            if (component != null)
                Setup(component.gameObject, pos, rot, parent, sourceType);
        }

        /// <summary>
        /// Applies transform and activation to the loaded object.
        /// </summary>
        private void Setup(GameObject obj, Vector3 pos, Quaternion rot, Transform parent, ObjectSourceType sourceType)
        {
            obj.transform.SetParent(parent, worldPositionStays: false);

            if (sourceType == ObjectSourceType.UIPooling)
                SetupUI(obj, pos, parent);
            else
                SetupNonUI(obj, pos, rot);
            
            obj.SetActive(true);
        }


        /// <summary>
        /// Sets transform state for non-UI objects spawned into the world.
        /// Applies world-space position and rotation directly to the GameObject.
        /// </summary>
        /// <param name="obj">The spawned GameObject to initialize.</param>
        /// <param name="pos">Target world position.</param>
        /// <param name="rot">Target world rotation.</param>
        private void SetupNonUI(GameObject obj, Vector3 pos, Quaternion rot)
        {
            obj.transform.position = pos;
            obj.transform.rotation = rot;
        }


        /// <summary>
        /// Sets transform state for UI objects (RectTransform) under a Canvas.
        /// Converts a world position to the appropriate UI-space position depending on the Canvas render mode,
        /// assigns the correct parent, and normalizes local rotation/scale.
        /// </summary>
        /// <param name="obj">The UI GameObject (must have a RectTransform).</param>
        /// <param name="worldPos">World-space position to project into UI space.</param>
        /// <param name="parent">The Canvas (or a child RectTransform) that will host the UI element.</param>
        private void SetupUI(GameObject obj, Vector3 worldPos, Transform parent)
        {
            RectTransform canvasRect = parent as RectTransform;
            RectTransform uiRect = obj.GetComponent<RectTransform>();

            if (canvasRect == null || uiRect == null)
            {
                Debug.LogWarning("[ObjectManager] UI Setup failed: Invalid RectTransform.");
                return;
            }

            obj.transform.SetParent(canvasRect, false);

            Camera cam = Camera.main;
            var canvas = canvasRect.GetComponentInParent<Canvas>();
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                cam = null;

            // World → Screen → Local UI
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // WorldSpace는 직접 worldPosition을 사용 가능
                uiRect.position = worldPos;
            }
            else
            {
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, screenPos, cam, out Vector2 anchoredPos))
                {
                    uiRect.anchoredPosition = anchoredPos;
                }
            }

            uiRect.localRotation = Quaternion.identity;
            uiRect.localScale = Vector3.one;
        }


        /// <summary>
        /// Returns a spawned object to its backing system based on the source it came from.
        /// Routes ObjectPooling/UIPooling to the ObjectPool and Addressables to Addressables.ReleaseInstance.
        /// Invokes <see cref="OnReturnRequested"/> after the return completes.
        /// </summary>
        /// <param name="key">The identifier used when the object was spawned.</param>
        /// <param name="component">A component on the instance being returned.</param>
        public void Return(string key, Component component)
        {
            if (!keyToSourceType.TryGetValue(key, out var sourceType))
            {
                Debug.LogWarning($"[ObjectManager] Unknown source type for key '{key}'. Defaulting to Pool.");
                sourceType = ObjectSourceType.ObjectPooling;
            }

            switch (sourceType)
            {
                case ObjectSourceType.ObjectPooling:
                case ObjectSourceType.UIPooling:
                    ObjectPool.Instance.ReturnToPool(key, component);
                    break;

                case ObjectSourceType.Addressables:
                    if (addressablesFactory is AddressableObjectFactory aFactory)
                        aFactory.ReturnObject(key, component);
                    else
                        Debug.LogWarning("[ObjectManager] Addressables factory is not set properly.");
                    break;
            }

            OnReturnRequested?.Invoke();
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
