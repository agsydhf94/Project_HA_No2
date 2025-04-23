using System.Collections;
using System.Collections.Generic;
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

    [CreateAssetMenu(fileName = "Buff effect", menuName = "DataSO/ItemEffect/Buff")]
    public class BuffEffectSO : ItemEffectSO
    {
        private PlayerStat stats;
        [SerializeField] private StatType buffType;
        [SerializeField] private int buffAmount;
        [SerializeField] private int buffDuration;

        public override void ExecuteEffect(Transform enemyPosition)
        {
            stats = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();
            stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
        }

        private Stat StatToModify()
        {
            if (buffType == StatType.Strength) return stats.strength;
            else if (buffType == StatType.Agility) return stats.agility;
            else if (buffType == StatType.Inteligence) return stats.inteligence;
            else if (buffType == StatType.Vitality) return stats.vitality;
            else if (buffType == StatType.Damage) return stats.damage;
            else if (buffType == StatType.CriticalChance) return stats.criticalChance;
            else if (buffType == StatType.CriticalPower) return stats.criticalPower;
            else if (buffType == StatType.Health) return stats.maxHp;
            else if (buffType == StatType.Armor) return stats.armor;
            else if (buffType == StatType.Evasion) return stats.evasion;
            else if (buffType == StatType.MagicResistance) return stats.magicResistance;
            else if (buffType == StatType.FireDamage) return stats.fireDamage;
            else if (buffType == StatType.IceDamage) return stats.iceDamage;
            else if (buffType == StatType.ShockDamage) return stats.ShockDamage;

            return null;
        }
    }
}
