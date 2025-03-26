using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class VFXArchive : SingletonBase<VFXArchive>
    {
        private ObjectPool objectPool;

        [Header("Sword Hit VFX")]
        [SerializeField] private ParticleSystem mari_SwordHit;

        public override void Awake()
        {
            objectPool = ObjectPool.Instance;

            // Sword Hit VFX
            objectPool.CreatePool("mari_SwordHit", mari_SwordHit, 2);
        }
    }
}
