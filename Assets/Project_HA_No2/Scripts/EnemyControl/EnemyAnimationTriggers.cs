using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyAnimationTriggers : MonoBehaviour
    {
        private EnemyBear enemyBear => GetComponent<EnemyBear>();

        private void AniomationTrigger_On()
        {
            enemyBear.AnimationFinishTrigger();
        }
    }
}
