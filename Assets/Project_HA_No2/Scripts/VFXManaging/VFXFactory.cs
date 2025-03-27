using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class VFXFactory : IVFXFactory
    {
        private ObjectPool pool;

        public VFXFactory(ObjectPool pool)
        {
            this.pool = pool;
        }

        public Component LoadFX(string key)
        {
            return pool.GetFromPool<Component>(key);
        }
    }
}
