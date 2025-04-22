using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStat : CharacterStats
    {
        private PlayerCharacter playerCharacter;
        private PlayerItemDrop playerItemDrop;

        protected override void Start()
        {
            base.Start();
            playerCharacter = GetComponent<PlayerCharacter>();
            playerItemDrop = GetComponent<PlayerItemDrop>();
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);            
        }

        protected override void Die()
        {
            base.Die();

            playerCharacter.Die();
            playerItemDrop.GenerateDropItem();
            
        }
    }
}
