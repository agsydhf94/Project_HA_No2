using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace HA
{
    public class BallThrowSkill : Skill
    {
        [Header("Skill Information")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform ballPosition;
        [SerializeField] private float throwSpeed = 15f; // ������ �⺻ �ӵ�
        [SerializeField] private float gravity = 9.81f;  // �߷� ��

        public GameObject createdBall;

        private void Awake()
        {
            ballPosition = GameObject.FindWithTag("BallPosition").transform;
        }

        public void CreateBall()
        {
            GameObject ball = Instantiate(ballPrefab, ballPosition.position, Quaternion.identity);
            ball.transform.SetParent(ballPosition);

            createdBall = ball;
        }

        public void ThrowBallWithTarget(GameObject ball, Transform target = null)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false; // ���� ����

            Vector3 launchVelocity;

            if (target != null)
            {
                launchVelocity = CalculateLaunchVelocity(target.position);
            }
            else
            {
                Vector3 forwardDir = GetForwardThrowDirection();
                float throwSpeed = 15f;
                launchVelocity = forwardDir * throwSpeed;
            }

            rb.velocity = launchVelocity;
        }


        public Vector3 CalculateLaunchVelocity(Vector3 targetPos)
        {
            Vector3 startPos = ballPosition.position;
            Vector3 toTarget = targetPos - startPos;

            float gravity = Mathf.Abs(Physics.gravity.y);
            float heightDifference = toTarget.y;

            // XZ ��� �Ÿ�
            toTarget.y = 0;
            float horizontalDistance = toTarget.magnitude;

            float angle = 45f * Mathf.Deg2Rad;

            float denominator = 2 * (heightDifference - Mathf.Tan(angle) * horizontalDistance) * Mathf.Pow(Mathf.Cos(angle), 2);

            // ���� ó��: �и� 0 �Ǵ� �����̸� ���� �Ұ���
            if (Mathf.Approximately(denominator, 0f) || denominator < 0f)
            {
                Debug.LogWarning("������ ��� ����: ���� �Ұ� �Ǵ� �и� ����");
                return toTarget.normalized * 5f + Vector3.up * 2f; // fallback
            }

            float velocitySquared = (gravity * horizontalDistance * horizontalDistance) / denominator;
            float velocity = Mathf.Sqrt(velocitySquared);

            Vector3 dir = toTarget.normalized;
            Vector3 launchVelocity = dir * velocity * Mathf.Cos(angle);
            launchVelocity.y = velocity * Mathf.Sin(angle);

            return launchVelocity;
        }


        public Vector3 GetForwardThrowDirection(float distance = 10f)
        {
            // ȭ�� �߾ӿ��� �������� Ray ���
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // ���� Ray�� ���𰡿� �¾Ҵٸ�
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                return (hit.point - ballPosition.position).normalized;
            }
            else
            {
                // �ƹ��͵� �� �¾Ҵٸ� �׳� ī�޶� �������� ���� �Ÿ� ��
                Vector3 fallbackPoint = ray.GetPoint(distance);
                return (fallbackPoint - ballPosition.position).normalized;
            }
        }
    }
}
