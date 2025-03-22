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

                // HomingProjectile ������Ʈ�� �ִٸ� Ÿ�� ����
                HomingProjectile homing = ball.GetComponent<HomingProjectile>();
                if (homing != null)
                {
                    homing.SetTarget(target);
                }
            }
            else
            {
                Vector3 forwardDir = GetForwardThrowDirection();
                float throwSpeed = 15f;
                launchVelocity = forwardDir * throwSpeed;
            }

            rb.velocity = launchVelocity;
        }


        public Vector3 CalculateLaunchVelocity(Vector3 targetPos, float timeToTarget = 0.5f)
        {
            Vector3 startPos = ballPosition.position;
            Vector3 toTarget = targetPos - startPos;

            Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
            float distanceXZ = toTargetXZ.magnitude;

            float velocityY = toTarget.y / timeToTarget + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;
            float velocityXZ = distanceXZ / timeToTarget;

            Vector3 launchVelocity = toTargetXZ.normalized * velocityXZ;
            launchVelocity.y = velocityY;

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
