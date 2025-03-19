using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class BallThrowSkill : Skill
    {
        [Header("Skill Information")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform ballPosition;
        [SerializeField] private float throwForce;

        private void Awake()
        {
            ballPosition = GameObject.FindWithTag("BallPosition").transform;
        }

        public void CreateBall()
        {
            GameObject ball = Instantiate(ballPrefab, ballPosition.position, Quaternion.identity);
            ball.transform.SetParent(ballPosition);

            /*
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false; // 물리 적용
            rb.velocity = ballPosition.forward * throwForce; // 공 던지기
            */
        }



    }
}
