using UnityEngine;

namespace HA
{
    /// <summary>
    /// ScriptableObject that defines parameters for a camera shake effect.
    /// Used by <see cref="ProfiledCameraShaker"/> to drive shake behavior over time.
    /// </summary>
    [CreateAssetMenu(fileName = "ShakeProfile", menuName = "DataSO/ShakeProfile")]
    public class ShakeProfileSO : ScriptableObject
    {
        /// <summary>
        /// Total duration of the shake in seconds.
        /// </summary>
        public float duration = 0.4f;

        /// <summary>
        /// Maximum magnitude of the shake displacement.
        /// </summary>
        public float strength = 1.2f;

        /// <summary>
        /// Frequency of the shake oscillation (in Hz).
        /// </summary>
        public float frequency = 12f;

        /// <summary>
        /// Local-space direction of the shake (e.g., (1,1,0) shakes X and Y).
        /// </summary>
        public Vector3 direction = new Vector3(1, 1, 0);

        /// <summary>
        /// Curve controlling intensity over time (e.g., fade out toward the end).
        /// </summary>
        public AnimationCurve intensityCurve = AnimationCurve.Linear(0, 1, 1, 0);
    }
}
