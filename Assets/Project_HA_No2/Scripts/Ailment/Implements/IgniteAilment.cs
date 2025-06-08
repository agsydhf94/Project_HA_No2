using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Ignite ailment that applies damage over time at fixed intervals.
    /// </summary>
    public class IgniteAilment : Ailment
    {
        public override string ID => "Ignite";

        /// <summary>
        /// Time between each damage tick.
        /// </summary>
        private float tickInterval = 0.3f;

        /// <summary>
        /// Countdown to the next tick.
        /// </summary>
        private float tickTimer;

        /// <summary>
        /// Amount of damage dealt per tick.
        /// </summary>
        private int igniteDamage;


        /// <summary>
        /// Constructs a new IgniteAilment.
        /// </summary>
        /// <param name="damage">Damage per tick.</param>
        /// <param name="duration">Total duration of the ailment.</param>
        public IgniteAilment(int damage, float duration) : base(duration)
        {
            this.igniteDamage = damage;
        }


        /// <summary>
        /// Called once when the ignite effect is first applied.
        /// Initializes the tick timer.
        /// </summary>
        /// <param name="target">Target receiving the ignite effect.</param>
        public override void OnApply(CharacterStats target)
        {
            tickTimer = tickInterval;
            Debug.Log($"{target.name} is ignited for {duration} seconds!");
        }


        /// <summary>
        /// Called every frame while the effect is active.
        /// Applies periodic damage based on tick interval.
        /// </summary>
        /// <param name="target">Target affected by ignite.</param>
        /// <param name="deltaTime">Time elapsed since last frame.</param>
        public override void OnTick(CharacterStats target, float deltaTime)
        {
            tickTimer -= deltaTime;
            if (tickTimer <= 0f)
            {
                target.DecreaseHealth(igniteDamage);
                Debug.Log($"Ignite tick: {igniteDamage} damage to {target.name}");

                if (target.IsDead)
                    target.Die();

                tickTimer = tickInterval;
            }
        }


        /// <summary>
        /// Called when the ignite effect expires or is removed.
        /// </summary>
        /// <param name="target">Target that was ignited.</param>
        public override void OnExpire(CharacterStats target)
        {
            Debug.Log($"{target.name} is no longer ignited.");
        }
    }
}
