using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkillController : MonoBehaviour
    {
        public void SetUpClone(Transform newTransform)
        {
            transform.position = newTransform.position;
        }
    }
}
