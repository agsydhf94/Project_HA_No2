using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ThrowBallSkillController : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private SphereCollider sphereCollider;
        private PlayerCharacter playerCharacter;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            rigidbody = GetComponent<Rigidbody>();
        }

        public void SetUpBall(Vector3 direction)
        {
            rigidbody.velocity = direction;
        }

    }
}
