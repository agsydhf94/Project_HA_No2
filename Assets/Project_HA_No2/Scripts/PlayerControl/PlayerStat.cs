using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStat : CharacterStats
    {
        private PlayerCharacter playerCharacter;

        protected override void Start()
        {
            base.Start();
            playerCharacter = GetComponent<PlayerCharacter>();
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);

            playerCharacter.DamageEffect();

            
        }
    }
}
