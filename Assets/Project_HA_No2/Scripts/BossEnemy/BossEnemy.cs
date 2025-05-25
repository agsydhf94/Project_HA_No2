using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace HA
{
    public class BossEnemy : MonoBehaviour
    {
        // Segment 파괴 절차
        public delegate void DestroySequence_Start();
        public DestroySequence_Start destroySequence_Start;

        public static Action<(Transform, int)> destroySequence_Grounded;
        public static Action<ParticleSystem> bossProjectile_BackToPool;

        [Header("Object and VFX Managing")]
        private ObjectManager objectManager;
        private IObjectSpawner objectSpawner;

        private PlayerCharacter playerCharacter;
        private Transform playerTransform;
        public float currentBossPosition;
        public Transform bossHead; // 보스의 머리 Transform
        public float headTurnSpeed = 2f; // 머리가 회전하는 속도
        public EnemySegment[] segments;

        private EnemyStat enemyCartStat;
        private float currentHP;
        public Canvas bossHPBar;

        [Header("Attack Settings")]
        public GameObject[] projectilePrefab; // 발사체 프리팹
        public Transform firePoint; // 발사체 발사 위치
        public float projectileSpeed; // 발사체 속도

        [Header("Cinemachine Settings")]
        public CinemachineSmoothPath cinemachineSmoothPath;
        public CinemachineDollyCart dollyCart; // DollyCart로 현재 위치 추적
        public int[] attackWaypoints;
        private int lastWaypointIndex = -1; // 이전 웨이포인트 저장


        [Header("Boss Attack Properties")]
        public float bossHealth = 100f; // 보스 체력
        public float attackInterval = 5f;
        private Vector3 attackDirection;
        private float attackProjectile_Speed;

        [Header("Laser Beam")]
        public GameObject rayStartObject;
        public Transform rayStartTransform; // 레이캐스팅이 시작되는 위치
        public GameObject beamLength_ByScale; // 맞는 지점과의 거리를 고려하여 Beam 오브젝트의 스케일 조정
        public ParticleSystem bossWarningCirele;
        public Vector3 bossWarningCircle_PosCorrection;
        private float beamHitDistance;


        private bool warningStartFlag = false;
        private bool isWarningCircleOn = false;
        [SerializeField] private float elapsedTime;
        public float warningTime;
        private int dummyCounter;
        public static Dictionary<int, DetachedSegment> detachedSegment = new Dictionary<int, DetachedSegment>();

        public Vector3 rayStart;
        public Vector3 characterPos;

        private void Awake()
        {
            enemyCartStat = GetComponent<EnemyStat>();
        }

        private void Start()
        {
            if(objectSpawner == null)
            {
                objectManager = ObjectManager.Instance;
                InitializeSpawner(objectManager);
            }

            foreach (var segment in segments)
            {
                enemyCartStat.currentHp += segment.characterStats.currentHp;
            }

            dummyCounter = 0;

            destroySequence_Grounded -= SecondExplosion;
            destroySequence_Grounded += SecondExplosion;

            playerCharacter = PlayerManager.Instance.playerCharacter;
        }

        void Update()
        {
            playerTransform = playerCharacter.transform;
            rayStartTransform = rayStartObject.transform;
            rayStart = rayStartTransform.position;


            RotateHeadTowardsPlayer();
            CheckWaypointAndAttack();


            if (elapsedTime >= warningTime)
            {
                elapsedTime = 0f;
                isWarningCircleOn = false;
                warningStartFlag = false;
            }


            rayStartObject.transform.LookAt(playerTransform);
            beamLength_ByScale.transform.localScale = new Vector3(0.01666667f, 0.01666667f, beamHitDistance);

        }

        private void InitializeSpawner(IObjectSpawner _objectSpawner)
        {
            objectSpawner = _objectSpawner;
        }


        private void CheckWaypointAndAttack()
        {
            if (dollyCart == null || cinemachineSmoothPath == null) return;

            // 마지막으로 지난 가장 최근의 웨이포인트 인덱스 계산
            int currentWaypointIndex = GetLastPassedWaypoint();
            //Debug.Log($"현재 위치 인덱스 : {currentWaypointIndex}");

            if (Array.Exists(attackWaypoints, x => x == currentWaypointIndex) && !warningStartFlag)
            {
                warningStartFlag = true;
                Debug.Log("경고 개시!!!!!!!!!!!!!!!!!!!!!!!");
                LaserWarningAndAttack();
            }
        }


        private int GetLastPassedWaypoint()
        {
            if (cinemachineSmoothPath.m_Waypoints.Length == 0)
            {
                Debug.LogWarning("웨이포인트가 설정되지 않았습니다.");
                return -1;
            }

            int left = 0;
            int right = cinemachineSmoothPath.m_Waypoints.Length - 1;
            int lastWaypointIndex = 0;
            float cartPosition = dollyCart.m_Position;

            while (left <= right)
            {
                int mid = left + (right - left) / 2; // 중간 인덱스 계산
                float waypointStartPos = cinemachineSmoothPath.FromPathNativeUnits(mid, CinemachinePathBase.PositionUnits.Distance);
                float waypointEndPos = (mid + 1 < cinemachineSmoothPath.m_Waypoints.Length)
                    ? cinemachineSmoothPath.FromPathNativeUnits(mid + 1, CinemachinePathBase.PositionUnits.Distance)
                    : float.MaxValue; // 마지막 웨이포인트인 경우, 끝을 무한대로 설정


                // 현재 위치가 이 웨이포인트 범위 안에 있는 경우
                if (cartPosition >= waypointStartPos && cartPosition < waypointEndPos)
                {
                    lastWaypointIndex = mid;
                    break;
                }
                else if (cartPosition < waypointStartPos)
                {
                    right = mid - 1; // 왼쪽 범위로 이동
                }
                else
                {
                    left = mid + 1; // 오른쪽 범위로 이동
                }
            }

            //Debug.Log($"마지막으로 지난 웨이포인트: {lastWaypointIndex}");
            return lastWaypointIndex;
        }


        public void UpdateTotalHealth()
        {
            int totalHealth = 0;
            foreach (var segment in segments)
            {
                totalHealth += segment.characterStats.currentHp;
            }
            enemyCartStat.currentHp = totalHealth;

            // 보스가 죽으면 전체 제거
            if (enemyCartStat.currentHp <= 0)
            {
                Die();
            }
        }

        private void RotateHeadTowardsPlayer()
        {
            // 플레이어 방향 계산
            Vector3 directionToPlayer = (playerTransform.position - bossHead.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            bossHead.rotation = Quaternion.Slerp(bossHead.rotation, targetRotation, Time.deltaTime * headTurnSpeed);
        }

        private float GetCurrentAttackInterval()
        {
            return attackInterval;
        }

        private void PerformAttack()
        {
            ProjectileAttack();
        }


        private void ProjectileAttack()
        {
            // 보스 머리가 플레이어를 바라보도록 회전
            var targetDirection = playerTransform.position - rayStartTransform.position;
            RotateHeadTowardsPlayer();

            var projectile = objectSpawner.Spawn("boss_ProjectileA", firePoint.position, Quaternion.identity).GetComponent<VisualEffect>();
            projectile.transform.LookAt(playerTransform);
            Debug.Log("Projectile 발사");
            
            var bossProjectileCompoenent = projectile.GetComponent<BossProjectile>();
            bossProjectileCompoenent.impactFlag = false;
            bossProjectileCompoenent.SetTargetDirection(targetDirection, projectileSpeed);
            bossProjectileCompoenent.Initialize(objectManager, enemyCartStat);

            Quaternion rotationAdjust = Quaternion.Euler(0, -90, 0);
            projectile.transform.rotation *= rotationAdjust;

        }

        

        

        // Segment Destroy Sequence 에 관련된 메서드

        public void SegmentHide_And_SetDummy(int index, Transform lastTransform)
        {

            var segment = segments[index];
            segment.segmentRenderer.enabled = false;
            segment.sphereCollider.enabled = false;

            var dummyComponent = objectSpawner.Spawn("bossEnemySegment_Dummy", lastTransform.position, Quaternion.identity);
            var dummySegment = dummyComponent.GetComponent<DetachedSegment>();
            dummySegment.key = dummyCounter;
            dummySegment.AddComponent<Rigidbody>();
            dummySegment.AddComponent<SphereCollider>();
            detachedSegment.Add(dummyCounter, dummySegment);

            dummyCounter++;

            StartCoroutine(segment.FirstExplosion(lastTransform));

        }

        private async void SecondExplosion((Transform, int) infoTuple)
        {
            var momentOiImpact_Transform = infoTuple.Item1;
            var key = infoTuple.Item2;

            var explosionEffect = ObjectPool.Instance.GetFromPool<ParticleSystem>("BossSegmentExplosionPrefab");
            explosionEffect.transform.position = momentOiImpact_Transform.position;

            Debug.Log($"Accessing key: {key}");
            var dummySegment = detachedSegment[key];
            dummySegment.key = 0;
            ObjectPool.Instance.ReturnToPool("bossEnemySegmenmt_Dummy", dummySegment);
            detachedSegment.Remove(key);

            await ExplosionEffect_Dequeue(explosionEffect);
        }

        public async UniTask ExplosionEffect_Dequeue(ParticleSystem explosionEffect)
        {
            await UniTask.Delay(4000);

            ObjectPool.Instance.ReturnToPool("BossSegmentExplosionPrefab", explosionEffect);
        }



        private async void LaserWarningAndAttack()
        {
            beamLength_ByScale.SetActive(true);
            Debug.Log("경고빔 활성화!");

            await LaserBeam();

            PerformAttack();
        }

        private async UniTask LaserBeam()
        {
            while (elapsedTime < warningTime)
            {
                // 보스에서 플레이어 방향으로 레이캐스트 실행
                Vector3 directionToPlayer = (playerTransform.position - rayStartTransform.position).normalized;
                Physics.Raycast(rayStartTransform.position, directionToPlayer, out RaycastHit hit, 10000, LayerMask.GetMask("Ground"));
                beamHitDistance = hit.distance;

                // 경고 서클이 아직 활성화되지 않았다면 오브젝트 풀에서 가져옴
                if (!isWarningCircleOn)
                {
                    var particleEffect = ObjectPool.Instance.GetFromPool<ParticleSystem>("bossWarningCircle");
                    bossWarningCirele = particleEffect;
                    isWarningCircleOn = true;
                }
                // 경고 서클의 위치와 회전 설정 (지면에 맞춰 배치)
                bossWarningCirele.transform.position = hit.point + bossWarningCircle_PosCorrection;
                bossWarningCirele.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(rayStartTransform.forward, hit.normal), hit.normal);

                await UniTask.Yield(); // 다음 프레임까지 대기
                elapsedTime += Time.deltaTime;

            }

            ObjectPool.Instance.ReturnToPool("bossWarningCircle", bossWarningCirele);

            beamLength_ByScale.SetActive(false);
            Debug.Log("경고빔 비활성화");
        }

        public void Die()
        {
            // 엔딩 시퀀스 시작
        }

        

        private void OnDrawGizmos()
        {
            if (rayStartTransform == null || playerTransform == null) return;

            // 레이 방향 계산
            Vector3 directionToPlayer = (playerTransform.position - rayStartTransform.position).normalized;

            // Raycast 실행 (충돌 검사)
            if (Physics.Raycast(rayStartTransform.position, directionToPlayer, out RaycastHit hit, 10000))
            {
                // 충돌된 지점까지 빨간색으로 선 표시
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rayStartTransform.position, hit.point);

                // 충돌된 지점에 구 표시 (반지름 0.2)
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.2f);
            }
            else
            {
                // 충돌되지 않았을 경우, 최대 거리까지 녹색 선 표시
                Gizmos.color = Color.green;
                Gizmos.DrawLine(rayStartTransform.position, rayStartTransform.position + directionToPlayer * 10000);
            }
        }
    }
}
