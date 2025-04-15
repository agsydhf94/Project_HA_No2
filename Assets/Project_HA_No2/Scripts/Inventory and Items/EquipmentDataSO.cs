using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }

    [CreateAssetMenu(fileName = "EquipmentData", menuName = "DataSO/Equipment")]
    public class EquipmentDataSO : ItemDataSO
    {
        public EquipmentType equipmentType;

        [Header("Main Stats")]
        public int strength;
        public int agility;
        public int intelligence;
        public int vitality;

        [Header("Attack Stats")]
        public int damage;
        public int criticalChance;
        public int criticalPower;

        [Header("Defense Stats")]
        public int maxHp;
        public int armor;
        public int evasion;
        public int magicResistance;

        [Header("Magic Stats")]
        public int fireDamage;
        public int iceDamage;
        public int lightingDamage;

        [Header("Craft Requirements")]
        public List<InventoryItem> requirementsForCraft;

        public void AddModifiers()
        {
            PlayerStat playerStats = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();

            playerStats.strength.AddModifier(strength);
            playerStats.agility.AddModifier(agility);
            playerStats.inteligence.AddModifier(intelligence);
            playerStats.vitality.AddModifier(vitality);

            playerStats.damage.AddModifier(damage);
            playerStats.criticalChance.AddModifier(criticalChance);
            playerStats.criticalPower.AddModifier(criticalPower);

            playerStats.maxHp.AddModifier(maxHp);
            playerStats.armor.AddModifier(armor);
            playerStats.evasion.AddModifier(evasion);
            playerStats.magicResistance.AddModifier(magicResistance);

            playerStats.fireDamage.AddModifier(fireDamage);
            playerStats.iceDamage.AddModifier(iceDamage);
            playerStats.lightingDamage.AddModifier(lightingDamage);
        }

        public void RemoveModifiers()
        {
            PlayerStat playerStats = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();

            playerStats.strength.RemoveModifier(strength);
            playerStats.agility.RemoveModifier(agility);
            playerStats.inteligence.RemoveModifier(intelligence);
            playerStats.vitality.RemoveModifier(vitality);

            playerStats.damage.RemoveModifier(damage);
            playerStats.criticalChance.RemoveModifier(criticalChance);
            playerStats.criticalPower.RemoveModifier(criticalPower);

            playerStats.maxHp.RemoveModifier(maxHp);
            playerStats.armor.RemoveModifier(armor);
            playerStats.evasion.RemoveModifier(evasion);
            playerStats.magicResistance.RemoveModifier(magicResistance);

            playerStats.fireDamage.RemoveModifier(fireDamage);
            playerStats.iceDamage.RemoveModifier(iceDamage);
            playerStats.lightingDamage.RemoveModifier(lightingDamage);
        }
    }
}
