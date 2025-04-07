using UnityEngine;

namespace HA
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Main Stats")]
        public Stat strength;     // 1 point : 공격 데미지 +1, 크리티컬 파워 +1%
        public Stat agility;      // 1 point : 회피 증가, 크리티컬 회피 +1%
        public Stat inteligence;  // 1 point : 마법 데미지 +1, 마법 저항력 + 3%
        public Stat vitality;     // 1 point : 최대 체력 +5

        [Header("Defense Stats")]
        public Stat maxHp;
        public Stat armor;
        public Stat evasion;

        public Stat damage;

        [SerializeField] private int currentHp;

        protected virtual void Start()
        {
            currentHp = maxHp.GetValue();
        }

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if (TargetCanAvoidAttack(targetStats))
                return;
            
            int totalDamage = damage.GetValue() + strength.GetValue();



            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);
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

            if (Random.Range(1, 100) < totalEvasion)
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
    }
}
