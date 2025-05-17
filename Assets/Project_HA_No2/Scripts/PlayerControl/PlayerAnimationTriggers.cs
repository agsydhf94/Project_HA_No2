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
        private Inventory inventory;

        private void Awake()
        {
            inventory = Inventory.Instance;
        }

        public void AnimationTrigger()
        {
            playerCharacter.AnimationFinishTrigger();
        }

        public void AnimationTrigger_Sub()
        {
            playerCharacter.AnimationFinishTrigger_Sub();
        }

        private void AttackTrigger()
        {
            List<Collider> colliders = ObjectDetection.GetObjectsBy<IDamagable>(playerCharacter.attackCheck, playerCharacter.attackCheckRadius);
            foreach(var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    // 추후 WeaponEffect 메서드가 이 부분을 대체할 것으로 보임
                    // entityVFX.SwordHitVFX();

                    var target = collider.transform.GetComponent<EnemyStat>();
                    if (target != null)
                        playerCharacter.characterStats.DoDamage(target);


                    // 플레이어가 가지고 있는 무기에 해당하는 이펙트를 재생하기 위해
                    // 인벤토리에서 정보를 받는다                    
                    WeaponEffect(target.transform);
                }
            }
        }

        public void WeaponEffect(Transform target)
        {
            EquipmentDataSO weaponDataSO = inventory.GetEquipment(EquipmentType.Weapon);
            weaponDataSO?.PlayEffect(target);
        }

        
        private void CreateBallOnHand()
        {
            SkillManager.Instance.ballThrowSkill.CreateBall();
        }

        
    }
}
