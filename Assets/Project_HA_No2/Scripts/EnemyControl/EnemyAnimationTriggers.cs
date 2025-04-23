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

        private void AttackTrigger()
        {
            List<Collider> colliders = ObjectDetection.GetObjectsBy<PlayerCharacter>(enemyBear.attackCheck, enemyBear.attackCheckRadius);
            foreach(var collider in colliders)
            {
                if(collider.TryGetComponent(out IDamagable damagable))
                {
                    var target = collider.transform.GetComponent<PlayerStat>();
                    enemyBear.characterStats.DoDamage(target);
                }
            }
        }

        private void OpenCounterWindow() => enemyBear.OpenCounterAttackWindow();
        private void CloseCounterWindow() => enemyBear.CloseCounterAttackWindow();

    }
}
