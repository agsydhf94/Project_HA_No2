using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Handles the ejection of shell casings from a specified point on the weapon.
    /// Applies randomized force and torque to simulate natural shell ejection.
    /// </summary>
    public class ShellEjector : MonoBehaviour
    {
        /// <summary>
        /// The point from which the shell casing will be ejected.
        /// </summary>
        public Transform ejectPoint;

        /// <summary>
        /// The base force applied to the shell during ejection.
        /// </summary>
        [SerializeField] private float ejectForce = 2f;

        /// <summary>
        /// The amount of random torque applied to the shell during ejection.
        /// </summary>
        [SerializeField] private float ejectTorque = 1f;


        /// <summary>
        /// Ejects a shell casing prefab using a key to identify the object from the object pool.
        /// Applies impulse force and torque to simulate realistic ejection motion.
        /// </summary>
        /// <param name="shellKey">The key used to retrieve the shell prefab from the object pool.</param>
        public void Eject(string shellKey)
        {
            Component shellComponent = ObjectManager.Instance.Spawn(
                shellKey,
                ejectPoint.position,
                ejectPoint.rotation,
                null,
                ObjectSourceType.ObjectPooling
            );

            if (shellComponent != null)
            {
                GameObject shellGO = shellComponent.gameObject;

                if (shellGO.TryGetComponent(out Rigidbody rb))
                {
                    // Generate slight randomness to the ejection direction
                    Vector3 randomDir = ejectPoint.right + new Vector3(
                        Random.Range(-0.1f, 0.1f),
                        Random.Range(0f, 0.1f),
                        Random.Range(-0.1f, 0.1f)
                    );

                    rb.AddForce(randomDir.normalized * ejectForce, ForceMode.Impulse);
                    rb.AddTorque(Random.insideUnitSphere * ejectTorque, ForceMode.Impulse);
                }
            }
        }
    }
}
