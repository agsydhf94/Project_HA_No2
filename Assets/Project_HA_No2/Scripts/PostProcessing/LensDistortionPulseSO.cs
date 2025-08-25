using UnityEngine;

namespace HA
{
    /// <summary>
    /// ScriptableObject defining the parameters for a lens distortion pulse effect.
    /// </summary>
    [CreateAssetMenu(fileName = "LensDistortionPulse", menuName = "DataSO/LensDistortionPulse")]
    public class LensDistortionPulseSO : ScriptableObject
    {
        [Header("Pulse")]
        /// <summary>
        /// Peak distortion intensity. Negative values create barrel distortion,
        /// positive values create pincushion distortion.
        /// </summary>
        [Range(-1f, 1f)] public float peakIntensity;

        /// <summary>
        /// Duration of the distortion rising phase.
        /// </summary>
        public float upTime;     

        /// <summary>
        /// Duration to hold the distortion at peak intensity.
        /// </summary>
        public float holdTime;

        /// <summary>
        /// Duration of the distortion returning phase.
        /// </summary>
        public float downTime;

        [Header("Shape (optional)")]
        /// <summary>
        /// Horizontal distortion multiplier.
        /// </summary>
        [Range(0f, 2f)] public float xMultiplier;

        /// <summary>
        /// Vertical distortion multiplier.
        /// </summary>
        [Range(0f, 2f)] public float yMultiplier;

        /// <summary>
        /// Horizontal center point of the distortion effect (0–1 range).
        /// </summary>
        [Range(0f, 1f)] public float centerX;

        /// <summary>
        /// Vertical center point of the distortion effect (0–1 range).
        /// </summary>
        [Range(0f, 1f)] public float centerY;

        /// <summary>
        /// Scale factor of the distortion effect.
        /// </summary>
        [Range(0.01f, 1.5f)] public float scale;
    }
}
