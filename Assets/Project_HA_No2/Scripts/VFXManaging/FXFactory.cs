using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class FXFactory : IFXFactory
    {
        private ObjectPool pool;

        public FXFactory(ObjectPool pool)
        {
            this.pool = pool;
        }

        public Component CreateFX(string key)
        {
            return pool.GetFromPool<Component>(key);
        }
    }
}
