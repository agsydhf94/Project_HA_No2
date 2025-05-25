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
        // Segment �ı� ����
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
        public Transform bossHead; // ������ �Ӹ� Transform
        public float headTurnSpeed = 2f; // �Ӹ��� ȸ���ϴ� �ӵ�
        public EnemySegment[] segments;

        private EnemyStat enemyCartStat;
        private float currentHP;
        public Canvas bossHPBar;

        [Header("Attack Settings")]
        public GameObject[] projectilePrefab; // �߻�ü ������
        public Transform firePoint; // �߻�ü �߻� ��ġ
        public float projectileSpeed; // �߻�ü �ӵ�

        [Header("Cinemachine Settings")]
        public CinemachineSmoothPath cinemachineSmoothPath;
        public CinemachineDollyCart dollyCart; // DollyCart�� ���� ��ġ ����
        public int[] attackWaypoints;
        private int lastWaypointIndex = -1; // ���� ��������Ʈ ����


        [Header("Boss Attack Properties")]
        public float bossHealth = 100f; // ���� ü��
        public float attackInterval = 5f;
        private Vector3 attackDirection;
        private float attackProjectile_Speed;

        [Header("Laser Beam")]
        public GameObject rayStartObject;
        public Transform rayStartTransform; // ����ĳ������ ���۵Ǵ� ��ġ
        public GameObject beamLength_ByScale; // �´� �������� �Ÿ��� ����Ͽ� Beam ������Ʈ�� ������ ����
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

            // ���������� ���� ���� �ֱ��� ��������Ʈ �ε��� ���
            int currentWaypointIndex = GetLastPassedWaypoint();
            //Debug.Log($"���� ��ġ �ε��� : {currentWaypointIndex}");

            if (Array.Exists(attackWaypoints, x => x == currentWaypointIndex) && !warningStartFlag)
            {
                warningStartFlag = true;
                Debug.Log("��� ����!!!!!!!!!!!!!!!!!!!!!!!");
                LaserWarningAndAttack();
            }
        }


        private int GetLastPassedWaypoint()
        {
            if (cinemachineSmoothPath.m_Waypoints.Length == 0)
            {
                Debug.LogWarning("��������Ʈ�� �������� �ʾҽ��ϴ�.");
                return -1;
            }

            int left = 0;
            int right = cinemachineSmoothPath.m_Waypoints.Length - 1;
            int lastWaypointIndex = 0;
            float cartPosition = dollyCart.m_Position;

            while (left <= right)
            {
                int mid = left + (right - left) / 2; // �߰� �ε��� ���
                float waypointStartPos = cinemachineSmoothPath.FromPathNativeUnits(mid, CinemachinePathBase.PositionUnits.Distance);
                float waypointEndPos = (mid + 1 < cinemachineSmoothPath.m_Waypoints.Length)
                    ? cinemachineSmoothPath.FromPathNativeUnits(mid + 1, CinemachinePathBase.PositionUnits.Distance)
                    : float.MaxValue; // ������ ��������Ʈ�� ���, ���� ���Ѵ�� ����


                // ���� ��ġ�� �� ��������Ʈ ���� �ȿ� �ִ� ���
                if (cartPosition >= waypointStartPos && cartPosition < waypointEndPos)
                {
                    lastWaypointIndex = mid;
                    break;
                }
                else if (cartPosition < waypointStartPos)
                {
                    right = mid - 1; // ���� ������ �̵�
                }
                else
                {
                    left = mid + 1; // ������ ������ �̵�
                }
            }

            //Debug.Log($"���������� ���� ��������Ʈ: {lastWaypointIndex}");
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

            // ������ ������ ��ü ����
            if (enemyCartStat.currentHp <= 0)
            {
                Die();
            }
        }

        private void RotateHeadTowardsPlayer()
        {
            // �÷��̾� ���� ���
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
            // ���� �Ӹ��� �÷��̾ �ٶ󺸵��� ȸ��
            var targetDirection = playerTransform.position - rayStartTransform.position;
            RotateHeadTowardsPlayer();

            var projectile = objectSpawner.Spawn("boss_ProjectileA", firePoint.position, Quaternion.identity).GetComponent<VisualEffect>();
            projectile.transform.LookAt(playerTransform);
            Debug.Log("Projectile �߻�");
            
            var bossProjectileCompoenent = projectile.GetComponent<BossProjectile>();
            bossProjectileCompoenent.impactFlag = false;
            bossProjectileCompoenent.SetTargetDirection(targetDirection, projectileSpeed);
            bossProjectileCompoenent.Initialize(objectManager, enemyCartStat);

            Quaternion rotationAdjust = Quaternion.Euler(0, -90, 0);
            projectile.transform.rotation *= rotationAdjust;

        }

        

        

        // Segment Destroy Sequence �� ���õ� �޼���

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
            Debug.Log("���� Ȱ��ȭ!");

            await LaserBeam();

            PerformAttack();
        }

        private async UniTask LaserBeam()
        {
            while (elapsedTime < warningTime)
            {
                // �������� �÷��̾� �������� ����ĳ��Ʈ ����
                Vector3 directionToPlayer = (playerTransform.position - rayStartTransform.position).normalized;
                Physics.Raycast(rayStartTransform.position, directionToPlayer, out RaycastHit hit, 10000, LayerMask.GetMask("Ground"));
                beamHitDistance = hit.distance;

                // ��� ��Ŭ�� ���� Ȱ��ȭ���� �ʾҴٸ� ������Ʈ Ǯ���� ������
                if (!isWarningCircleOn)
                {
                    var particleEffect = ObjectPool.Instance.GetFromPool<ParticleSystem>("bossWarningCircle");
                    bossWarningCirele = particleEffect;
                    isWarningCircleOn = true;
                }
                // ��� ��Ŭ�� ��ġ�� ȸ�� ���� (���鿡 ���� ��ġ)
                bossWarningCirele.transform.position = hit.point + bossWarningCircle_PosCorrection;
                bossWarningCirele.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(rayStartTransform.forward, hit.normal), hit.normal);

                await UniTask.Yield(); // ���� �����ӱ��� ���
                elapsedTime += Time.deltaTime;

            }

            ObjectPool.Instance.ReturnToPool("bossWarningCircle", bossWarningCirele);

            beamLength_ByScale.SetActive(false);
            Debug.Log("���� ��Ȱ��ȭ");
        }

        public void Die()
        {
            // ���� ������ ����
        }

        

        private void OnDrawGizmos()
        {
            if (rayStartTransform == null || playerTransform == null) return;

            // ���� ���� ���
            Vector3 directionToPlayer = (playerTransform.position - rayStartTransform.position).normalized;

            // Raycast ���� (�浹 �˻�)
            if (Physics.Raycast(rayStartTransform.position, directionToPlayer, out RaycastHit hit, 10000))
            {
                // �浹�� �������� ���������� �� ǥ��
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rayStartTransform.position, hit.point);

                // �浹�� ������ �� ǥ�� (������ 0.2)
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.2f);
            }
            else
            {
                // �浹���� �ʾ��� ���, �ִ� �Ÿ����� ��� �� ǥ��
                Gizmos.color = Color.green;
                Gizmos.DrawLine(rayStartTransform.position, rayStartTransform.position + directionToPlayer * 10000);
            }
        }
    }
}
