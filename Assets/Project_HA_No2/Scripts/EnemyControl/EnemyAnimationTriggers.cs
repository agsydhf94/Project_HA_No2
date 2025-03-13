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
            List<Collider> colliders = CharacterBase.ObjectDetection<PlayerCharacter>(enemyBear.attackCheck, enemyBear.attackCheckRadius);
            foreach(var collider in colliders)
            {
                if(collider.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.ApplyDamage();
                }
            }
        }

        private void OpenCounterWindow() => enemyBear.OpenCounterAttackWindow();
        private void CloseCounterWindow() => enemyBear.CloseCounterAttackWindow();

    }
}
