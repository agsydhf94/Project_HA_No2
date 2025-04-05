using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ElementSkillController : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation *= Quaternion.Euler(0f, Time.deltaTime * 35f, 0f);
        }
    }
}
