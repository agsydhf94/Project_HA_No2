using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStat : CharacterStats
    {
        private PlayerCharacter playerCharacter;
        private PlayerItemDrop playerItemDrop;

        protected override void Start()
        {
            base.Start();
            playerCharacter = GetComponent<PlayerCharacter>();
            playerItemDrop = GetComponent<PlayerItemDrop>();
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);            
        }

        protected override void Die()
        {
            base.Die();

            playerCharacter.Die();
            playerItemDrop.GenerateDropItem();
            
        }

        protected override void DecreaseHealth(int damage)
        {
            base.DecreaseHealth(damage);

            EquipmentDataSO armor = Inventory.Instance.GetEquipment(EquipmentType.Armor);
            armor?.PlayEffect(playerCharacter.transform);

        }

        public override void OnEvasion()
        {
            playerCharacter.skillManager.dodgeSkill.CreateCloneOnDodge();
        }

        public void CloneDoDamage(CharacterStats targetStats, float multiplier)
        {
            if (TargetCanAvoidAttack(targetStats))
                return;

            
            int totalDamage = damage.GetValue() + strength.GetValue();

            if (multiplier > 0)
            {
                totalDamage = Mathf.RoundToInt(totalDamage * multiplier);
            }

            if (CanCritical())
            {
                totalDamage = CalculateCriticalDamage(totalDamage);
            }



            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);

            // �κ��丮���� ���� �������� ���Ⱑ ���� ������ ���� ���� ���� ������ ���ϱ�
            DoMagicalDamage(targetStats);
        }
    }
}
