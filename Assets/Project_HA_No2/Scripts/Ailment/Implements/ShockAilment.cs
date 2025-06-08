using System.Collections;
using System.Collections.Generic;
using HA;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Ailment that represents an electric shock effect.
    /// If re-applied while already active, it triggers a chain lightning attack.
    /// </summary>
    public class ShockAilment : Ailment
    {
        public override string ID => "Shock";

        /// <summary>
        /// Damage to apply when the shock strike is triggered.
        /// </summary>
        private int shockDamage;

        /// <summary>
        /// Prefab to instantiate when triggering the shock strike.
        /// Should be assigned externally via a factory or injection.
        /// </summary>
        private GameObject shockStrikePrefab;


        /// <summary>
        /// Constructor for creating a ShockAilment instance.
        /// </summary>
        /// <param name="damage">Damage dealt by the shock strike.</param>
        /// <param name="duration">Duration of the shock ailment.</param>
        public ShockAilment(int damage, float duration) : base(duration)
        {
            this.shockDamage = damage;
        }


        /// <summary>
        /// Called once when the ailment is applied.
        /// Logs the shock state.
        /// </summary>
        /// <param name="target">The character being affected.</param>
        public override void OnApply(CharacterStats target)
        {
            Debug.Log($"{target.name} is shocked.");
        }


         /// <summary>
        /// Called every frame while the ailment is active.
        /// If the shock ailment is re-applied while already active, 
        /// triggers a shock strike to the nearest enemy.
        /// </summary>
        /// <param name="target">The affected character.</param>
        /// <param name="deltaTime">Elapsed time since last update.</param>
        public override void OnTick(CharacterStats target, float deltaTime)
        {
            // 감전 중 재적용 시 연쇄 번개 공격
            if (target.HasAilment(ID)) // 외부에서 감지해 IsAlreadyShocked일 때 재적용된 경우
            {
                TriggerShockStrike(target);
                timer = 0; // 즉시 만료
            }
        }


         /// <summary>
        /// Called once when the ailment expires.
        /// </summary>
        /// <param name="target">The character whose shock effect expired.</param>
        public override void OnExpire(CharacterStats target)
        {
            Debug.Log($"{target.name} is no longer shocked.");
        }


        /// <summary>
        /// Finds the nearest enemy and triggers a shock strike effect on them.
        /// </summary>
        /// <param name="source">The shocked character initiating the strike.</param>
        private void TriggerShockStrike(CharacterStats source)
        {
            bool skipSelf = true;
            var closestEnemy = FindClosestEnemy.GetClosestEnemy(source.transform, skipSelf);

            if (closestEnemy == null)
                closestEnemy = source.transform;

            var newShock = Object.Instantiate(shockStrikePrefab, source.transform.position + Vector3.up, Quaternion.identity);
            if (newShock.TryGetComponent(out ShockStrikeController controller))
            {
                var targetStats = closestEnemy.GetComponent<CharacterStats>();
                controller.ThunderStrikeSetup(shockDamage, targetStats);
            }
        }
    }
}
