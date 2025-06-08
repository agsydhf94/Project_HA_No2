using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Base class for all status effects (ailments) such as Ignite, Chill, and Shock.
    /// Manages lifecycle timing, tick updates, and expiration behavior.
    /// </summary>
    public abstract class Ailment
    {
        /// <summary>
        /// Unique identifier for the ailment (e.g., "Ignite", "Chill", "Shock").
        /// </summary>
        public abstract string ID { get; }

        /// <summary>
        /// Whether the ailment is currently active.
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Total duration of the ailment effect in seconds.
        /// </summary>
        protected float duration;

        /// <summary>
        /// Countdown timer that decreases each frame.
        /// </summary>
        protected float timer;


        /// <summary>
        /// Constructs a new ailment with a specified duration.
        /// </summary>
        /// <param name="duration">The duration of the effect in seconds.</param>
        public Ailment(float duration)
        {
            this.duration = duration;
        }


        /// <summary>
        /// Applies the ailment to the target, initializing timers and triggering the effect.
        /// </summary>
        /// <param name="target">The character receiving the ailment.</param>
        /// <param name="duration">Optional override for the duration.</param>
        public virtual void Apply(CharacterStats target, float duration)
        {
            this.duration = duration;
            timer = duration;
            IsActive = true;
            OnApply(target);
        }


        /// <summary>
        /// Updates the ailment each frame. Handles tick behavior and expiration.
        /// </summary>
        /// <param name="target">The target affected by the ailment.</param>
        /// <param name="deltaTime">Time passed since last update.</param>
        public void Update(CharacterStats target, float deltaTime)
        {
            if (!IsActive) return;

            timer -= deltaTime;
            if (timer <= 0f)
            {
                IsActive = false;
                OnExpire(target);
            }
            else
            {
                OnTick(target, deltaTime);
            }
        }

        /// <summary>
        /// Called once when the ailment is first applied.
        /// Use this to activate visual effects, debuffs, etc.
        /// </summary>
        public abstract void OnApply(CharacterStats target);

        /// <summary>
        /// Called every frame while the ailment is active.
        /// Useful for damage-over-time or periodic effects.
        /// </summary>
        public abstract void OnTick(CharacterStats target, float deltaTime);

        /// <summary>
        /// Called when the ailment expires or is removed.
        /// Cleanup or effect reversal logic goes here.
        /// </summary>
        public abstract void OnExpire(CharacterStats target);
    }
}
