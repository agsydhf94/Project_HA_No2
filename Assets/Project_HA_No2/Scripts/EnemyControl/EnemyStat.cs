using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyStat : CharacterStats
    {
        private Enemy enemy;

        protected override void Start()
        {
            base.Start();
        }

        public override void TakeDamage(int _damage)
        {
            base.TakeDamage(_damage);

            enemy.DamageEffect();
        }
    }
}
