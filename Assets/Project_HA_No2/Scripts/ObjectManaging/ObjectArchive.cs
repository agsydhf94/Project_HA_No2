using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ObjectArchive : MonoBehaviour
    {
        private ObjectPool objectPool;

        [Header("Player Ball Skill")]
        [SerializeField] private ThrownBall skillBall;
        [SerializeField] private BulletProjectile bulletTrajectoryStick;

        private void Awake()
        {
            objectPool = ObjectPool.Instance;

            // Player Ball Skill
            objectPool.CreatePool("skillBall", skillBall, 2);
            objectPool.CreatePool("bulletTrajectoryStick", bulletTrajectoryStick, 20);
        }
    }
}
