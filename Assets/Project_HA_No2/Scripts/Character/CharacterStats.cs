using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CharacterStats : MonoBehaviour
    {
        public Stat strength;
        public Stat damage;
        public Stat maxHp;

        [SerializeField] private int currentHp;

        protected virtual void Start()
        {
            currentHp = maxHp.GetValue();
        }

        public virtual void DoDamage(CharacterStats targetStats)
        {
            int totalDamage = damage.GetValue() + strength.GetValue();
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

    }
}
