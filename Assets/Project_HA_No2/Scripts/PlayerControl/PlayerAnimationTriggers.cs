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
                    // ���� WeaponEffect �޼��尡 �� �κ��� ��ü�� ������ ����
                    // entityVFX.SwordHitVFX();

                    var target = collider.transform.GetComponent<EnemyStat>();
                    if (target != null)
                        playerCharacter.characterStats.DoDamage(target);


                    // �÷��̾ ������ �ִ� ���⿡ �ش��ϴ� ����Ʈ�� ����ϱ� ����
                    // �κ��丮���� ������ �޴´�                    
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
