using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace HA
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        [SerializeField] private GameObject attackUIPrefab;
        private SkillManager skillManager;

        private float maxSize;
        private float growSpeed;
        private float shrinkSpeed;

        private bool canGrow;
        private bool canShrink;
        private bool canAttack;

        public int amountOfAttacks = 4;
        public float cloneAttackCooldown = 0.3f;
        private float cloneAttackTimer;

        public List<Enemy> attackedTargets = new List<Enemy>();
        public Queue<Enemy> detectedTargets = new Queue<Enemy>();

        public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttacks, float cloneAttackCooldown)
        {
            this.maxSize = maxSize;
            this.growSpeed = growSpeed;
            this.shrinkSpeed = shrinkSpeed;
            this.amountOfAttacks = amountOfAttacks;
            this.cloneAttackCooldown = cloneAttackCooldown;
        }

        private void Awake()
        {
            skillManager = SkillManager.Instance;
        }

        private void Update()
        {
            cloneAttackTimer -= Time.deltaTime;

            if (cloneAttackTimer < 0)
            {
                cloneAttackTimer = cloneAttackCooldown;
            }

            if (canGrow && !canShrink)
            {
                transform.localScale
                = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), growSpeed * Time.deltaTime);
            }
            if (canShrink)
            {
                transform.localScale
                = Vector3.Lerp(transform.localScale, new Vector3(-1, -1, -1), shrinkSpeed * Time.deltaTime);

                if (transform.localScale.x < 0)
                {
                    Destroy(gameObject);
                }
            }

            AttackByClone();
        }

        private void AttackByClone()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var enemy = detectedTargets.Dequeue();
                AddEnemyToList(enemy);
                enemy.blackHoleFlag.SetActive(false);
                skillManager.cloneSkill.CreateClone(enemy.transform, Vector3.zero);

                amountOfAttacks--;
                if (amountOfAttacks <= 0)
                {
                    canAttack = false;
                    canShrink = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.FreezeTime(true);
                enemy.blackHoleFlag.SetActive(true);
                detectedTargets.Enqueue(enemy);
                Debug.Log(enemy.gameObject.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out Enemy enemy))
            {
                enemy.FreezeTime(false);
            }
        }

        public void AddEnemyToList(Enemy enemyTransform) => attackedTargets.Add(enemyTransform);
    }
}
