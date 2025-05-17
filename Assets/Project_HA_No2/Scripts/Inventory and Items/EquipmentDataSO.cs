using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Magic,
        Potion
    }

    public enum WeaponType
    {
        Sword,
        Rifle,
        HandGun,
        Grenade,
        NonWeapon,
    }

    public struct WeaponData
    {
        // Rifle
        public float _fireRate;
        public int _magazineCurrent;
        public int _magazineCapacity;

        // Grenade
        public float _explosionRadius;
    }

    [CreateAssetMenu(fileName = "EquipmentData", menuName = "DataSO/Equipment")]
    public class EquipmentDataSO : ItemDataSO
    {
        public EquipmentType equipmentType;
        public WeaponType weaponType;

        [System.Serializable]
        public class PartEntry
        {
            public string partSlotName;
            public GameObject partPrefab;
            public string attachBoneName;

            public Vector3 localPosition;
            public Vector3 localRotation;
            public Vector3 localScale;
        }
        // Equipment Prefab
        public List<PartEntry> parts = new();

        

        [Header("WeaponData - Rifle")]
        public float fireRate;
        public int magazine_Current;
        public int magazine_Capacity;

        [Header("WeaponData - Grenade")]
        public float explosionRadius;

        [Header("Unique Effect")]
        public float itemCoolDown;
        public ItemEffectSO[] itemEffects;


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
        public int shockDamage;

        [Header("Craft Requirements")]
        public List<InventoryItem> requirementsForCraft;

        [Header("ToolTip UI Information")]
        private int descriptionLength;

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
            playerStats.ShockDamage.AddModifier(shockDamage);
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
            playerStats.ShockDamage.RemoveModifier(shockDamage);
        }

        public void PlayEffect(Transform targetTransform)
        {
            foreach(var vfx in itemEffects)
            {
                vfx.ExecuteEffect(targetTransform);
            }
        }

        public override string GetDescription()
        {
            sb.Length = 0;
            descriptionLength = 0;

            AddItemDescription(strength, "Strength");
            AddItemDescription(agility, "Agility");
            AddItemDescription(intelligence, "Inteligence");
            AddItemDescription(vitality, "Vitality");

            AddItemDescription(damage, "Damage");
            AddItemDescription(criticalChance, "Critical Chance");
            AddItemDescription(criticalPower, "Critical Power");

            AddItemDescription(maxHp, "MaxHP");
            AddItemDescription(evasion, "Evasion");
            AddItemDescription(armor, "Armor");
            AddItemDescription(magicResistance, "Magic Resist");

            AddItemDescription(fireDamage, "Fire Damage");
            AddItemDescription(iceDamage, "Ice Damage");
            AddItemDescription(shockDamage, "Shock Damage");

            sb.AppendLine();
            sb.AppendLine();
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].effectDescription.Length > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("* " + itemEffects[i].effectDescription);
                    sb.Append("");
                }
            }

            if(descriptionLength < 5)
            {
                for(int i = 0; i < 5 - descriptionLength; i++)
                {
                    sb.AppendLine();
                    sb.Append("");
                }
            }

            

            return sb.ToString();
        }

        private void AddItemDescription(int value, string name)
        {
            if(value != 0)
            {
                if(sb.Length > 0)
                {
                    sb.AppendLine();
                }
                if(value > 0)
                {
                    sb.Append("+ " + value + " " + name);
                }

                descriptionLength++;
            }
        }

        public WeaponData TransferWeaponData()
        {
            WeaponData data = new WeaponData();

            data._fireRate = fireRate;
            data._magazineCurrent = magazine_Current;
            data._magazineCapacity = magazine_Capacity;

            data._explosionRadius = explosionRadius;

            return data;
        }
    }
}
