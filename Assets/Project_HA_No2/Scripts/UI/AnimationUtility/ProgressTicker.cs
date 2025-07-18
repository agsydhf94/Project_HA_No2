using UnityEngine;
using System;

namespace HA
{
    /// <summary>
    /// Generates a time-based progression value, shaped by an AnimationCurve.
    /// 
    /// Useful for driving progress-based UI or effects such as:
    /// - Skill charging indicators
    /// - Cooldown visuals
    /// - Custom animation feedback
    /// 
    /// The progression value is evaluated every frame based on elapsed time,
    /// and emitted through the <see cref="onProgressTick"/> event using the configured curve.
    /// 
    /// Timing and curve behavior can be configured via the inspector or dynamically at runtime.
    /// </summary>
    public class ProgressTicker : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private AnimationCurve progressCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// Called every frame with the current curve-shaped progress value (usually 0 to 1).
        /// </summary>
        public Action<float> onProgressTick;

        /// <summary>
        /// Called once when progression completes.
        /// </summary>
        public Action onComplete;

        private float timeElapsed;
        private bool isRunning;


        /// <summary>
        /// Starts the progress ticker.
        /// You may override the duration for this run by passing a value.
        /// </summary>
        /// <param name="durationOverride">Optional duration for this run (overrides the default).</param>
        public void StartTicker(float durationOverride = -1f)
        {
            timeElapsed = 0f;
            isRunning = true;
            if (durationOverride > 0f) duration = durationOverride;
        }


        /// <summary>
        /// Stops the ticker and resets the internal time.
        /// </summary>
        public void StopTicker()
        {
            isRunning = false;
            timeElapsed = 0f;
        }


        /// <summary>
        /// Updates the internal timer and evaluates the progression curve every frame while the ticker is running.
        /// 
        /// - Invokes <see cref="onProgressTick"/> with the current curved value (0~1)
        /// - Stops and invokes <see cref="onComplete"/> once the duration is fully elapsed
        /// </summary>
        void Update()
        {
            if (!isRunning) return;

            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            float curvedValue = progressCurve.Evaluate(t);
            onProgressTick?.Invoke(curvedValue);

            if (t >= 1f)
            {
                isRunning = false;
                onComplete?.Invoke();
            }
        }
    }
}
