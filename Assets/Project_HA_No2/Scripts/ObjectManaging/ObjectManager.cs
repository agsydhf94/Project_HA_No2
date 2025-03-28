using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ObjectManager : SingletonBase<ObjectManager>
    {
        private IObjectFactory factory;

        public override void Awake()
        {
            factory = new ObjectFactory(ObjectPool.Instance);
        }

        public T SpawnObject<T>(string key, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            Component component = factory.CreateObject(key, position, rotation, parent);
            return component as T;
        }

        public void DespawnObject(string key, Component component)
        {
            if (component == null) return;
            component.transform.SetParent(this.transform);
            ObjectPool.Instance.ReturnToPool(key, component);
        }
    }
}
