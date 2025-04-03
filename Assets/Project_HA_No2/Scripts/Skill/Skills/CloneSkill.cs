using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkill : Skill
    {
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private bool canAttack;

        public void CreateClone(Transform targetTransform, Vector3 offset)
        {
            GameObject newClone = Instantiate(clonePrefab);
            newClone.GetComponent<CloneSkillController>().SetUpClone(targetTransform, canAttack, offset);

            Vector3 position = Random.onUnitSphere;
            position.y = 0;
            position.Normalize();

            Vector3 direction = targetTransform.transform.position - newClone.transform.position;

            newClone.transform.forward = direction;
            newClone.transform.position = targetTransform.transform.position + position;

        }
    }
}
