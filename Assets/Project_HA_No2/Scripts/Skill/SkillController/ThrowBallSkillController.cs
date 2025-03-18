using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ThrowBallSkillController : MonoBehaviour
    {
        private SphereCollider sphereCollider;
        private PlayerCharacter playerCharacter;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

    }
}
