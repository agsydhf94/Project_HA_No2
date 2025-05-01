using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class CloneSkill : Skill
    {
        [Header("Clone Information")]
        [SerializeField] private float attackMultiplier;
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float cloneDuration;

        [Header("Clone Attack")]
        [SerializeField] SkillTreeSlotUI cloneAttack_Unlock;
        [SerializeField] private float cloneAttackMultiplier;
        [SerializeField] private bool canAttack;

        [Header("Aggressive Clone")]
        [SerializeField] private SkillTreeSlotUI aggressiveClone_Unlock;
        [SerializeField] private float aggressiveCloneAttackMultiplier;
        public bool canApplyOnHitEffect { get; private set; }


        [Header("Multiple Clones")]
        [SerializeField] private SkillTreeSlotUI multipleClone_Unlock;
        [SerializeField] private float multiCloneAttackMultiplier;
        [SerializeField] public bool canDuplicateClone;
        [SerializeField] private float chanceOfDuplicate;

        [Header("Element Instead of Clone")]
        [SerializeField] private SkillTreeSlotUI elementInsteadOfClone_Unlock;
        [SerializeField] private bool canCreateElementInsteadOfClone;


        protected override void Start()
        {
            base.Start();

            cloneAttack_Unlock.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
            aggressiveClone_Unlock.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
            multipleClone_Unlock.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
            elementInsteadOfClone_Unlock.GetComponent<Button>().onClick.AddListener(UnlockElementInsteadClone);
        }

        #region Unlock Skill

        private void UnlockCloneAttack()
        {
            if (cloneAttack_Unlock.unlocked)
            {
                canAttack = true;
                attackMultiplier = cloneAttackMultiplier;
            }                
        }

        private void UnlockAggressiveClone()
        {
            if (aggressiveClone_Unlock.unlocked)
            {
                canApplyOnHitEffect = true;
                attackMultiplier = aggressiveCloneAttackMultiplier;
            }
        }

        private void UnlockMultiClone()
        {
            if (multipleClone_Unlock.unlocked)
            {
                canDuplicateClone = true;
                attackMultiplier = multiCloneAttackMultiplier;
            }
        }

        private void UnlockElementInsteadClone()
        {
            if (elementInsteadOfClone_Unlock.unlocked)
            {
                canCreateElementInsteadOfClone = true;
            }
        }

        #endregion

        public void CreateClone(Transform targetTransform, Vector3 offset)
        {
            if(canCreateElementInsteadOfClone)
            {
                skillManager.elementSkill.CreateElement();
                skillManager.elementSkill.CurrentElementChooseRandomTarget();
                return;
            }

            GameObject newClone = Instantiate(clonePrefab);
            CloneSkillController newController = newClone.GetComponent<CloneSkillController>();
            var closestEnemy = FindClosestEnemy.GetClosestEnemy(newClone.transform);
            newController.SetUpClone(newClone, targetTransform, closestEnemy, cloneDuration, canAttack, offset, canDuplicateClone, chanceOfDuplicate, attackMultiplier);

            Vector3 position = Random.onUnitSphere;
            position.y = 0;
            position.Normalize();

            Vector3 direction = targetTransform.transform.position - newClone.transform.position;

            newClone.transform.forward = direction;
            newClone.transform.position = targetTransform.transform.position + position;

        }

        

        

        public void CreateCloneWithDelay(Transform enemyTransform)
        {
            CreateCloneWithDelay_Task(enemyTransform, new Vector3(0f, 0f, 4f)).Forget(); // 비동기 호출
        }

        private async UniTask CreateCloneWithDelay_Task(Transform transform, Vector3 offset)
        {
            await UniTask.Delay(400); // 밀리초 단위: 0.4초
            CreateClone(transform, offset);
        }
    }
}
