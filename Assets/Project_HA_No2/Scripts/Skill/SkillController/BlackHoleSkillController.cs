using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        [SerializeField] private GameObject attackUIPrefab;

        public float maxSize;
        public float growSpeed;
        public bool canGrow;

        public List<Transform> attackedTargets = new List<Transform>();
        public Queue<Transform> detectedTargets = new Queue<Transform>();
        public KeyCode hotkey;

        private void Update()
        {
            transform.localScale
                = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize, maxSize), growSpeed * Time.deltaTime);  

            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.FreezeTime(true);
                enemy.blackHoleFlag.SetActive(true);
                detectedTargets.Enqueue(enemy.transform);
                Debug.Log(enemy.gameObject.name);

                BlackHoleSkill_AttackIcon attackIcon = enemy.blackHoleFlag.gameObject.GetComponent<BlackHoleSkill_AttackIcon>();
                attackIcon.SetupHotKey(hotkey, other.transform, this);
            }
        }

        public void AddEnemyToList(Transform enemyTransform) => attackedTargets.Add(enemyTransform);
    }
}
