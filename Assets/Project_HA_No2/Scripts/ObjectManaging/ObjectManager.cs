using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ObjectManager : SingletonBase<ObjectManager>, IObjectSpawner, IObjectReturn
    {
        private IObjectFactory factory;

        public event Action OnReturnRequested;

        public override void Awake()
        {
            factory = new ObjectFactory(ObjectPool.Instance);
        }

        /// <summary>
        /// 오브젝트를 로드하고 위치/회전까지 설정함
        /// </summary>

        public Component Spawn(string key, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var component = factory.LoadObject(key);
        if (component == null) return null;

        GameObject obj = component.gameObject;
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return component;
    }

        public void Return(string key, Component component)
        {
            ObjectPool.Instance.ReturnToPool(key, component);
        }

    }
}
