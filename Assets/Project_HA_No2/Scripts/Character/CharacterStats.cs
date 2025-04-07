using Unity.Mathematics;
using UnityEngine;

namespace HA
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Main Stats")]
        public Stat strength;     // 1 point : ���� ������ +1, ũ��Ƽ�� �Ŀ� +1%
        public Stat agility;      // 1 point : ȸ�� ����, ũ��Ƽ�� ȸ�� +1%
        public Stat inteligence;  // 1 point : ���� ������ +1, ���� ���׷� + 3%
        public Stat vitality;     // 1 point : �ִ� ü�� +5

        [Header("Attack Stats")]
        public Stat damage;
        public Stat criticalChance;
        public Stat criticalPower;     // Default : 150%

        [Header("Defense Stats")]
        public Stat maxHp;
        public Stat armor;
        public Stat evasion;
        public Stat magicResistance;

        [Header("Magic Stats")]
        public Stat fireDamage;
        public Stat iceDamage;
        public Stat lightingDamage;

        public bool isIgnited;  // �ð��� ������ ���� ������ 
        public bool isChilled;  // armor�� 20% ���δ�
        public bool isShocked;  // accuracy�� 20% ���δ�


        [SerializeField] private int currentHp;

        protected virtual void Start()
        {
            criticalPower.SetDefaultValue(150);
            currentHp = maxHp.GetValue();
        }

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if (TargetCanAvoidAttack(targetStats))
                return;

            int totalDamage = damage.GetValue() + strength.GetValue();

            if (CanCritical())
            {
                totalDamage = CalculateCriticalDamage(totalDamage);
            }



            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);
        }

        public virtual void DoMagicalDamage(CharacterStats targetStats)
        {
            int _fireDamage = fireDamage.GetValue();
            int _iceDamage = iceDamage.GetValue();
            int _lightingDamage = lightingDamage.GetValue();

            int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + inteligence.GetValue();
            totalMagicalDamage = CheckTargetResistance(targetStats, totalMagicalDamage);

            targetStats.TakeDamage(totalMagicalDamage);



            if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
                return;

            bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
            bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
            bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


            while(!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if(UnityEngine.Random.value < 0.5f && _fireDamage > 0)
                {
                    canApplyIgnite = true;
                    targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }

                if(UnityEngine.Random.value < 0.5f && _iceDamage > 0)
                {
                    canApplyChill = true;
                    targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }

                if (UnityEngine.Random.value < 0.5f && _lightingDamage > 0)
                {
                    canApplyShock = true;
                    targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }
            }

            targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

        }

        private int CheckTargetResistance(CharacterStats targetStats, int totalMagicalDamage)
        {
            totalMagicalDamage -= targetStats.magicResistance.GetValue() + (targetStats.inteligence.GetValue() * 3);
            totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
            return totalMagicalDamage;
        }

        public void ApplyAilments(bool ignite, bool chill, bool shock)
        {
            if (isIgnited || isChilled || isShocked)
                return;

            isIgnited = ignite;
            isChilled = chill;
            isShocked = shock;
        }


        public virtual void TakeDamage(int _damage)
        {
            currentHp -= _damage;

            if(currentHp < 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {

        }

        private bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

            if (UnityEngine.Random.Range(1, 100) < totalEvasion)
            {
                Debug.Log("Attack Avoided");
                return true;
            }
            return false;
        }
        private int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            totalDamage -= targetStats.armor.GetValue();
            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
            return totalDamage;
        }
        private bool CanCritical()
        {
            int totalCriticalChance = criticalChance.GetValue() + agility.GetValue();

            if(UnityEngine.Random.Range(1, 100) <= totalCriticalChance)
            {
                return true;
            }
            return false;
        }
        private int CalculateCriticalDamage(int damage)
        {
            float totalCriticalPower = (criticalPower.GetValue() + strength.GetValue()) * 0.01f;

            float criticalDamage = damage * totalCriticalPower;

            return Mathf.RoundToInt(criticalDamage);
        }
    }
}
