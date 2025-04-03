using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkill : Skill
    {
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private bool canAttack;

        public void CreateClone(Transform cloneTransform, Vector3 offset)
        {
            GameObject newClone = Instantiate(clonePrefab);
            newClone.GetComponent<CloneSkillController>().SetUpClone(cloneTransform, canAttack, offset);
        }
    }
}
