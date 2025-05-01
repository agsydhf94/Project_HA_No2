using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyStat : CharacterStats
    {
        private Enemy enemy;
        private ItemDrop itemDrop;

        [Header("Level information")]
        [SerializeField] private int level;

        [Range(0f, 1f)]
        [SerializeField] private float percentageModifier;

        protected override void Start()
        {
            ApplyModifiers();

            base.Start();
            enemy = GetComponent<Enemy>();
            itemDrop = GetComponent<ItemDrop>();
        }

        private void ApplyModifiers()
        {
            Modify(damage);
            Modify(criticalChance);
            Modify(criticalPower);

            Modify(maxHp);
            Modify(armor);
            Modify(evasion);
            Modify(magicResistance);

            Modify(fireDamage);
            Modify(iceDamage);
            Modify(ShockDamage);
        }

        private void Modify(Stat stat)
        {
            for(int i = 0; i < level; i++)
            {
                float modifier = stat.GetValue() * percentageModifier;

                stat.AddModifier(Mathf.RoundToInt(modifier));
            }
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);
        }

        protected override void Die()
        {
            base.Die();
            enemy.Die();
            itemDrop.GenerateDropItem();
        }
    }
}
