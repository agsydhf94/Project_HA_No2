using UnityEngine;

namespace HA
{
    /// <summary>
    /// Authoring class for DOTS-based Crystal objects.
    /// Defines initial velocity, custom gravity vector, and explosion force response settings.
    /// </summary>
    public class ObjectAuthoring : MonoBehaviour
    {
        public Vector3 initialVelocity = Vector3.zero;
        public Vector3 gravity = new Vector3(0, -9.81f, 0);
        public bool receiveExplosionForce = true;
    }
}

