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

        public Component LoadObject(string key)
        {
            return pool.GetFromPool<Component>(key);
        }
    }
}
