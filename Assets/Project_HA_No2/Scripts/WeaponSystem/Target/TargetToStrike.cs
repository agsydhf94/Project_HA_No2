
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Marks an object as a valid homing target for projectiles.
    /// Provides a reference point (typically the object's transform)
    /// that guided projectiles can use for tracking.
    /// </summary>
    public class TargetToStrike : MonoBehaviour
    {
        public Rigidbody rb;


        /// <summary>
        /// Returns the transform used as the target point for homing logic.
        /// </summary>
        public Transform GetTargetPoint() => transform;
    }
}

