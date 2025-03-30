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

        private void Awake()
        {
            objectPool = ObjectPool.Instance;

            // Player Ball Skill
            objectPool.CreatePool<ThrownBall>("skillBall", skillBall, 2);
        }
    }
}
