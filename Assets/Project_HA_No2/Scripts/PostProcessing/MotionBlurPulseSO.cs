using UnityEngine;

namespace HA
{
    /// <summary>
    /// ScriptableObject that defines parameters for a timed motion blur pulse.
    /// </summary>
    [CreateAssetMenu(fileName = "MotionBlurPulse", menuName = "DataSO/MotionBlurPulse")]
    public class MotionBlurPulseSO : ScriptableObject
    {
        [Header("Pulse")]
        /// <summary>
        /// Peak motion blur intensity during the pulse (0–1).
        /// </summary>
        [Range(0f, 1f)] public float peakIntensity;

        /// <summary>
        /// Duration for rising from 0 to peak intensity.
        /// </summary>
        public float upTime;

        /// <summary>
        /// Duration to hold at peak intensity.
        /// </summary>
        public float holdTime;

        /// <summary>
        /// Duration for returning from peak back to 0.
        /// </summary>
        public float downTime;

        [Header("Clamp (trail length limit)")]

        /// <summary>
        /// Limits perceived trail length (higher clamp = shorter trails).
        /// </summary>
        [Range(0f, 0.5f)] public float clamp;
    }
}
