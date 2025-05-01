using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace HA
{
    public enum StatType
    {
        Strength,
        Agility,
        Inteligence,
        Vitality,
        Damage,
        CriticalChance,
        CriticalPower,
        Health,
        Armor,
        Evasion,
        MagicResistance,
        FireDamage,
        IceDamage,
        ShockDamage
    }

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
        public Stat ShockDamage;

        public bool isIgnited;  // 시간이 지남에 따라 데미지 
        public bool isChilled;  // armor를 20% 줄인다
        public bool isShocked;  // accuracy를 20% 줄인다

        [SerializeField] private float ailmentsDuration;
        private float ignitedTimer;
        private float chillTimer;
        private float shockTimer;

        private float igniteDamageCooldown = 0.3f;
        private float igniteDamageTimer;
        private int igniteDamage;

        [SerializeField] private GameObject shockStrikePrefab;
        private int shockDamage;

        public int currentHp;

        public System.Action onHealthChanged;

        public bool isDead { get; set; }

        private CharacterBase characterBase;

        protected virtual void Start()
        {
            criticalPower.SetDefaultValue(150);
            currentHp = GetMaxHealthValue();

            characterBase = GetComponent<CharacterBase>();
            Debug.Log("캐릭터 스탯 Start 호출됨");
        }

        protected virtual void Update()
        {
            ignitedTimer -= Time.deltaTime;
            chillTimer -= Time.deltaTime;
            shockTimer -= Time.deltaTime;

            igniteDamageTimer -= Time.deltaTime;

            if (ignitedTimer < 0)
                isIgnited = false;

            if (chillTimer < 0)
                isChilled = false;

            if (shockTimer < 0)
                isShocked = false;

            if(isIgnited)
                ApplyIgniteDamage();
        }

        public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify)
        {
            StartCoroutine(StatModify(modifier, duration, statToModify));
        }

        private IEnumerator StatModify(int modifier, float duration, Stat statToModify)
        {
            statToModify.AddModifier(modifier);

            yield return new WaitForSeconds(duration);
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

            // 인벤토리에서 현재 장착중인 무기가 마법 공격이 있을 때만 마법 데미지 가하기
            DoMagicalDamage(targetStats);
        }

        #region Magical Damage and Ailments
        public virtual void DoMagicalDamage(CharacterStats targetStats)
        {
            int _fireDamage = fireDamage.GetValue();
            int _iceDamage = iceDamage.GetValue();
            int _lightingDamage = ShockDamage.GetValue();

            int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + inteligence.GetValue();
            totalMagicalDamage = CheckTargetResistance(targetStats, totalMagicalDamage);

            targetStats.TakeDamage(totalMagicalDamage);



            if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
                return;

            AttemptToApplyAilments(targetStats, _fireDamage, _iceDamage, _lightingDamage);

        }
        private void AttemptToApplyAilments(CharacterStats targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
        {
            bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
            bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
            bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


            while (!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if (UnityEngine.Random.value < 0.5f && _fireDamage > 0)
                {
                    canApplyIgnite = true;
                    targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }

                if (UnityEngine.Random.value < 0.5f && _iceDamage > 0)
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

            if (canApplyShock)
                targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * 0.2f));

            targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }       
        public void ApplyAilments(bool ignite, bool chill, bool shock)
        {
            bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
            bool canApplyChill = !isIgnited && !isChilled && !isShocked;
            bool canApplyShocked = !isIgnited && !isChilled;

            if (ignite && canApplyIgnite)
            {
                isIgnited = ignite;
                ignitedTimer = ailmentsDuration;
            }

            if (chill && canApplyChill)
            {
                isChilled = chill;
                chillTimer = ailmentsDuration;

                float slowPercentage = 0.4f;
                characterBase.GetSlowBy(slowPercentage, ailmentsDuration);
            }

            if (shock && canApplyShocked)
            {
                if(!isShocked)
                {
                    ApplyShock(shock);
                }
                else
                {
                    HitClosestTargetWithShockStrike();
                }
            }
        }
        public void ApplyShock(bool shock)
        {
            if (isShocked)
                return;

            isShocked = shock;
            shockTimer = ailmentsDuration;
        }
        private void HitClosestTargetWithShockStrike()
        {
            bool skipSelf = true;
            var closestEnemy = FindClosestEnemy.GetClosestEnemy(transform, skipSelf);

            if (closestEnemy == null)
                closestEnemy = transform;

            var newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.transform.position += new Vector3(0f, 1f, 0f);

            var newShockController = newShockStrike.GetComponent<ShockStrikeController>();
            var target = closestEnemy.GetComponent<CharacterStats>();
            newShockController.ThunderStrikeSetup(shockDamage, target);
        }
        private void ApplyIgniteDamage()
        {
            if (igniteDamageTimer < 0)
            {
                DecreaseHealth(igniteDamage);

                if (currentHp <= 0 && !isDead)
                    Die();

                igniteDamageTimer = igniteDamageCooldown;
            }
        }
        public void SetupIgniteDamage(int damage) => igniteDamage = damage;
        public void SetupShockStrikeDamage(int damage) => shockDamage = damage;

        #endregion

        #region Stat Calculation

        public virtual void OnEvasion()
        {

        }

        protected bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

            // 내가 shock에 빠지면 상대의 회피가 증가 (플레이어가 불리해지는 상황 연출)
            if (isShocked)
                totalEvasion += 20;

            if (UnityEngine.Random.Range(1, 100) < totalEvasion)
            {
                // 공격 회피됨
                targetStats.OnEvasion();
                return true;
            }
            return false;
        }
        protected int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            // 타겟이 얼면 타겟의 armor 성능이 20% 저하
            if(targetStats.isChilled)
                totalDamage -= Mathf.RoundToInt(targetStats.armor.GetValue() * 0.8f);
            else
                totalDamage -= targetStats.armor.GetValue();

            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
            return totalDamage;
        }

        private int CheckTargetResistance(CharacterStats targetStats, int totalMagicalDamage)
        {
            totalMagicalDamage -= targetStats.magicResistance.GetValue() + (targetStats.inteligence.GetValue() * 3);
            totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
            return totalMagicalDamage;
        }

        protected bool CanCritical()
        {
            int totalCriticalChance = criticalChance.GetValue() + agility.GetValue();

            if(UnityEngine.Random.Range(1, 100) <= totalCriticalChance)
            {
                return true;
            }
            return false;
        }
        protected int CalculateCriticalDamage(int damage)
        {
            float totalCriticalPower = (criticalPower.GetValue() + strength.GetValue()) * 0.01f;

            float criticalDamage = damage * totalCriticalPower;

            return Mathf.RoundToInt(criticalDamage);
        }

        public int GetMaxHealthValue()
        {
            return maxHp.GetValue() + vitality.GetValue() * 5;
        }

        #endregion

        public virtual void TakeDamage(int damage)
        {
            DecreaseHealth(damage);

            GetComponent<CharacterBase>().DamageImpact();

            // 이곳에서 vfx도 재생

            if (currentHp <= 0 && !isDead)
            {
                Die();
            }
        }

        public virtual void IncreaseHealthBy(int amount)
        {
            currentHp += amount;

            if (currentHp > GetMaxHealthValue())
            {
                currentHp = GetMaxHealthValue();
            }

            if(onHealthChanged != null)
            {
                onHealthChanged();
            }
        }

        protected virtual void DecreaseHealth(int damage)
        {
            currentHp -= damage;

            if (onHealthChanged != null)
            {
                onHealthChanged();
            }
        }

        protected virtual void Die()
        {
            isDead = true;
        }

        public Stat GetStat(StatType statType)
        {
            if (statType == StatType.Strength) return strength;
            else if (statType == StatType.Agility) return agility;
            else if (statType == StatType.Inteligence) return inteligence;
            else if (statType == StatType.Vitality) return vitality;
            else if (statType == StatType.Damage) return damage;
            else if (statType == StatType.CriticalChance) return criticalChance;
            else if (statType == StatType.CriticalPower) return criticalPower;
            else if (statType == StatType.Health) return maxHp;
            else if (statType == StatType.Armor) return armor;
            else if (statType == StatType.Evasion) return evasion;
            else if (statType == StatType.MagicResistance) return magicResistance;
            else if (statType == StatType.FireDamage) return fireDamage;
            else if (statType == StatType.IceDamage) return iceDamage;
            else if (statType == StatType.ShockDamage) return ShockDamage;

            return null;
        }
    }
}
