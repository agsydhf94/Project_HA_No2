using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        public float maxSize;
        public float growSpeed;
        public bool canGrow;

        public List<Transform> detectedTargets;

        private void Update()
        {
            transform.localScale
                = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), growSpeed * Time.deltaTime);  
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Enemy>() != null)
            {
                detectedTargets.Add(other.transform);
            }
        }
    }
}
