using System.Collections;
using System.Collections.Generic;
using HA;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Ailment that slows the target's movement speed for a certain duration.
    /// </summary>
    public class FrozeAilment : Ailment
    {
        public override string ID => "Chill";

        /// <summary>
        /// Percentage by which the target's movement speed is reduced.
        /// </summary>
        private float slowPercentage;

        /// <summary>
        /// Indicates whether the slow effect has been successfully applied.
        /// </summary>
        private bool slowApplied;


        /// <summary>
        /// Constructs a new FrozeAilment instance.
        /// </summary>
        /// <param name="duration">Total duration of the slow effect.</param>
        /// <param name="slowPercentage">Movement speed reduction (e.g., 0.4f = 40% slow).</param>
        public FrozeAilment(float duration, float slowPercentage = 0.4f) : base(duration)
        {
            this.slowPercentage = slowPercentage;
        }


        /// <summary>
        /// Called once when the ailment is applied.
        /// Applies a movement speed reduction to the target.
        /// </summary>
        /// <param name="target">The character being affected by the slow.</param>        
        public override void OnApply(CharacterStats target)
        {
            if (target.characterBase != null)
            {
                target.characterBase.GetSlowBy(slowPercentage, duration);
                slowApplied = true;
                Debug.Log($"{target.name} is chilled: -{slowPercentage * 100f}% speed for {duration} seconds.");
            }
        }


        /// <summary>
        /// Called every frame while the ailment is active.
        /// Chill does not apply any recurring effect, so this is empty.
        /// </summary>
        /// <param name="target">The affected character.</param>
        /// <param name="deltaTime">Elapsed time since last update.</param>
        public override void OnTick(CharacterStats target, float deltaTime)
        {
            // Chill does not require periodic updates.
        }


        /// <summary>
        /// Called once when the ailment duration ends or it is removed.
        /// Assumes the speed restoration is handled by the characterBase system.
        /// </summary>
        /// <param name="target">The character who was chilled.</param>
        public override void OnExpire(CharacterStats target)
        {
            Debug.Log($"{target.name} is no longer chilled.");
            // Movement speed recovery is assumed to be handled by characterBase.
        }
    }
}
