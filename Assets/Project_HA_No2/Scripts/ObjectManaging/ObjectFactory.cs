using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ObjectFactory : IObjectFactory
    {
        private ObjectPool pool;

        public ObjectFactory(ObjectPool pool)
        {
            this.pool = pool;
        }

        public Component CreateObject(string key, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var component = pool.GetFromPool<Component>(key);
            if (component == null) return null;

            GameObject obj = component.gameObject;
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return component;
        }
    }
}
