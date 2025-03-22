using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class HomingProjectile : MonoBehaviour
    {
        public Transform target;
        public float speed = 10f;
        public float rotateSpeed = 5f;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 newVelocity = Vector3.Lerp(rb.velocity, direction * speed, rotateSpeed * Time.deltaTime);

            rb.velocity = newVelocity;

            // 시각적으로도 타겟을 향하게 회전
            if (newVelocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(newVelocity);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
