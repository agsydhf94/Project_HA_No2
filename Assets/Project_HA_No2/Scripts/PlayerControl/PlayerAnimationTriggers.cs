using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        private PlayerCharacter playerCharacter => GetComponent<PlayerCharacter>();
        private EntityVFX entityVFX => GetComponent<EntityVFX>();


        private void AnimationTrigger()
        {
            playerCharacter.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            List<Collider> colliders = CharacterBase.ObjectDetection<IDamagable>(playerCharacter.attackCheck, playerCharacter.attackCheckRadius);
            foreach(var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    entityVFX.SwordHitVFX();
                    damagable.ApplyDamage();
                }
            }
        }

        
        private void CreateBallOnHand()
        {
            SkillManager.Instance.ballThrowSkill.CreateBall();
        }

        
    }
}
