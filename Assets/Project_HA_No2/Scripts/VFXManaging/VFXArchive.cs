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

        [Header("Ball Skill Hit VFX")]
        [SerializeField] private ParticleSystem mari_BallSkillHit;
        [SerializeField] private ParticleSystem mari_BallSkillHit_Final;

        public override void Awake()
        {
            objectPool = ObjectPool.Instance;

            // Sword Hit VFX
            objectPool.CreatePool("mari_SwordHit", mari_SwordHit, 2);

            // Ball Skill Hit VFX
            objectPool.CreatePool("mari_BallSkillHit", mari_BallSkillHit, 5);
            objectPool.CreatePool("mari_BallSkillHit_Final", mari_BallSkillHit_Final, 2);
        }
    }
}
