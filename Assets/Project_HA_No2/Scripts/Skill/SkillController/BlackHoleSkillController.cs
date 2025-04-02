using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace HA
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        [SerializeField] private GameObject attackUIPrefab;

        public float maxSize;
        public float growSpeed;
        public bool canGrow;

        public List<Enemy> attackedTargets = new List<Enemy>();
        public Queue<Enemy> detectedTargets = new Queue<Enemy>();

        private void Update()
        {
            transform.localScale
                = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), growSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                var enemy = detectedTargets.Dequeue();
                AddEnemyToList(enemy);
                enemy.blackHoleFlag.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.FreezeTime(true);
                enemy.blackHoleFlag.SetActive(true);
                detectedTargets.Enqueue(enemy);
                Debug.Log(enemy.gameObject.name);

                BlackHoleSkill_AttackIcon attackIcon = enemy.blackHoleFlag.gameObject.GetComponent<BlackHoleSkill_AttackIcon>();
                attackIcon.SetupHotKey(hotkey, other.transform, this);
            }
        }

        public void AddEnemyToList(Enemy enemyTransform) => attackedTargets.Add(enemyTransform);
    }
}
