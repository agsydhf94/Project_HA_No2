using Unity.Mathematics;
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

        public bool isIgnited;  // 시간이 지남에 따라 데미지 
        public bool isChilled;  // armor를 20% 줄인다
        public bool isShocked;  // accuracy를 20% 줄인다

        private float ignitedTimer;
        private float chillTimer;
        private float shockTimer;

        private float igniteDamageCooldown = 0.3f;
        private float igniteDamageTimer;
        private int igniteDamage;


        [SerializeField] private int currentHp;

        protected virtual void Start()
        {
            criticalPower.SetDefaultValue(150);
            currentHp = maxHp.GetValue();
        }

        protected virtual void Update()
        {
            ignitedTimer -= Time.deltaTime;
            chillTimer -= Time.deltaTime;
            shockTimer -= Time.deltaTime;

            igniteDamageTimer -= Time.deltaTime;

            if(ignitedTimer < 0)
                isIgnited = false;

            if(chillTimer < 0)
                isChilled = false;

            if(shockTimer < 0)
                isShocked = false;

            if(igniteDamageTimer < 0 && isIgnited)
            {
                Debug.Log("Take burn Damage" + igniteDamage);
                currentHp -= igniteDamage;

                if (currentHp < 0)
                    Die();

                igniteDamageTimer = igniteDamageCooldown;
            }
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
            // targetStats.TakeDamage(totalDamage);
            DoMagicalDamage(targetStats);
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

            if (canApplyIgnite)
                targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));

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

            if(ignite)
            {
                isIgnited = ignite;
                ignitedTimer = 5;
            }

            if (chill)
            {
                isChilled = chill;
                chillTimer = 3;
            }

            if (shock)
            {
                isShocked = shock;
                shockTimer = 2;
            }


        }

        public void SetupIgniteDamage(int damage) => igniteDamage = damage;


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

            // 내가 shock에 빠지면 상대의 회피가 증가 (플레이어가 불리해지는 상황 연출)
            if (isShocked)
                totalEvasion += 20;

            if (UnityEngine.Random.Range(1, 100) < totalEvasion)
            {
                Debug.Log("Attack Avoided");
                return true;
            }
            return false;
        }
        private int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            // 타겟이 얼면 타겟의 armor 성능이 20% 저하
            if(targetStats.isChilled)
                totalDamage -= Mathf.RoundToInt(targetStats.armor.GetValue() * 0.8f);
            else
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
