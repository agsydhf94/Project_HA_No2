using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkill : Skill
    {
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float cloneDuration;
        [SerializeField] private bool canAttack;

        [SerializeField] private bool createCloneDashStart;
        [SerializeField] private bool createCloneOnDashOver;
        [SerializeField] private bool canCreateCloneOnCounterAttack;
        [SerializeField] public bool canDuplicateClone;
        [SerializeField] private float chanceOfDuplicate;
 
        public void CreateClone(Transform targetTransform, Vector3 offset)
        {
            GameObject newClone = Instantiate(clonePrefab);
            SetUpClone(newClone, targetTransform, canAttack, offset, canDuplicateClone, chanceOfDuplicate);

            Vector3 position = Random.onUnitSphere;
            position.y = 0;
            position.Normalize();

            Vector3 direction = targetTransform.transform.position - newClone.transform.position;

            newClone.transform.forward = direction;
            newClone.transform.position = targetTransform.transform.position + position;

        }

        public void SetUpClone(GameObject newClone, Transform initialPosition, bool canAttack, Vector3 offset, bool canDuplicate, float chanceOfDuplicate)
        {
            if (canAttack)
            {
                newClone.GetComponent<Animator>().SetInteger("AttackNumber", Random.Range(1, 3));
            }

            transform.position = initialPosition.position + offset;

            var newCloneController = newClone.GetComponent<CloneSkillController>();
            newCloneController.duplicateBool = canDuplicate;
            newCloneController.chanceToDuplicate = chanceOfDuplicate;
            newCloneController.cloneTimer = cloneDuration;
        }

        public void CreateCloneOnDashStart()
        {
            if(createCloneDashStart)
            {
                CreateClone(playerCharacter.transform, Vector3.zero);
            }
        }

        public void CreateCloneOnDashOver()
        {
            if(createCloneOnDashOver)
            {
                CreateClone(playerCharacter.transform, Vector3.zero);
            }
        }

        public void CreateCloneOnCounterAttack(Transform enemyTransform)
        {
            if (canCreateCloneOnCounterAttack)
            {
                CreateCloneWithDelay(enemyTransform, new Vector3(0f, 0f, 4f)).Forget(); // 비동기 호출
            }
        }

        private async UniTask CreateCloneWithDelay(Transform transform, Vector3 offset)
        {
            await UniTask.Delay(400); // 밀리초 단위: 0.4초
            CreateClone(transform, offset);
        }
    }
}
