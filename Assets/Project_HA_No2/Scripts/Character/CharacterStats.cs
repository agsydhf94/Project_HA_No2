using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CharacterStats : MonoBehaviour
    {
        public Stat damage;
        public Stat maxHp;

        [SerializeField] private int currentHp;

        private void Start()
        {
            currentHp = maxHp.GetValue();
        }

        public void TakeDamage(int _damage)
        {
            currentHp -= _damage;

            if(currentHp < 0)
            {
                Die();
            }
        }

        private void Die()
        {

        }

    }
}
