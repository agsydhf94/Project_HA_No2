using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace HA
{
    public class BallThrowSkill : Skill
    {
        [Header("Skill Information")]
        [SerializeField] private SkillTreeSlotUI ballThrow_Unlock;
        public bool ballThrowUnlocked;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform ballPosition;
        [SerializeField] private float throwSpeed = 15f; // 던지는 기본 속도
        [SerializeField] private float gravity = 9.81f;  // 중력 값

        public GameObject createdBall;
        public IObjectSpawner objectSpawner;
        public ObjectPool objectPool;

        private void Awake()
        {
            ballPosition = GameObject.FindWithTag("BallPosition").transform;            
        }

        protected override void Start()
        {
            base.Start();

            ballThrow_Unlock.GetComponent<Button>().onClick.AddListener(UnlockThrowBall);
        }

        public void InitializeSpawner(IObjectSpawner spawner)
        {
            this.objectSpawner = spawner;
        }

        public void UnlockThrowBall()
        {
            if (ballThrow_Unlock.unlocked)
                ballThrowUnlocked = true;
        }

        public void CreateBall()
        {
            //GameObject ball = Instantiate(ballPrefab, ballPosition.position, Quaternion.identity);

            var ball = objectSpawner.Spawn("skillBall", ballPosition.position, Quaternion.identity);
            ball.transform.SetParent(ballPosition);
            createdBall = ball.gameObject;

            var thrownBallComponent = createdBall.GetComponent<ThrownBall>();
            thrownBallComponent.Initialize(ObjectManager.Instance);
            thrownBallComponent.playerCharacter = playerCharacter;
            thrownBallComponent.crashCount = 0;

        }

        public async UniTask ThrowBallWithTarget(GameObject ball, Transform target = null)
        {
            await UniTask.Yield(); // 프레임 대기 (부모 해제와 위치 반영 보장)

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false; // 물리 적용

            await UniTask.Yield();

            Vector3 launchVelocity;

            if (target != null)
            {
                launchVelocity = CalculateLaunchVelocity(target.position);

                // HomingProjectile 컴포넌트가 있다면 타겟 설정
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
            // 화면 중앙에서 정면으로 Ray 쏘기
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // 만약 Ray가 무언가에 맞았다면
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                return (hit.point - ballPosition.position).normalized;
            }
            else
            {
                // 아무것도 안 맞았다면 그냥 카메라 방향으로 일정 거리 앞
                Vector3 fallbackPoint = ray.GetPoint(distance);
                return (fallbackPoint - ballPosition.position).normalized;
            }
        }

    }
}
