using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ElementSkillController : MonoBehaviour
    {
        private float elementTimer;

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(0f, Time.deltaTime * 35f, 0f);

            elementTimer -= Time.deltaTime;
            if(elementTimer < 0)
            {
                SelfDestruct();
            }
        }

        public void SetupElement(float duration)
        {
            elementTimer = duration;
        }

        public void SelfDestruct() => Destroy(gameObject);
    }
}
