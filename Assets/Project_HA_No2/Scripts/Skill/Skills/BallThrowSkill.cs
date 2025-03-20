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
        [SerializeField] private float throwSpeed = 15f; // ������ �⺻ �ӵ�
        [SerializeField] private float gravity = 9.81f;  // �߷� ��

        private void Awake()
        {
            ballPosition = GameObject.FindWithTag("BallPosition").transform;
        }

        public void CreateBall()
        {
            GameObject ball = Instantiate(ballPrefab, ballPosition.position, Quaternion.identity);
            ball.transform.SetParent(ballPosition);

            // ��ǥ ��ġ�� �����Ͽ� ������ �ӵ��� ������
            Vector3 launchVelocity = CalculateLaunchVelocity(target.position);

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false; // ���� ����
            rb.velocity = launchVelocity;

        }

        private Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - ballPosition.position;
            float distance = direction.magnitude; // ��ǥ������ �Ÿ�

            float heightDifference = targetPosition.y - ballPosition.position.y;
            float timeToTarget = Mathf.Sqrt(2 * heightDifference / gravity) + (distance / throwSpeed);

            Vector3 horizontalVelocity = (direction / timeToTarget);
            Vector3 verticalVelocity = Vector3.up * gravity * timeToTarget * 0.5f;

            return horizontalVelocity + verticalVelocity;
        }



    }
}
