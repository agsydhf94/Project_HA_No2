using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkill : Skill
    {
        [SerializeField] private GameObject clonePrefab;

        public void CreateClone()
        {
            GameObject newClone = Instantiate(clonePrefab);
        }
    }
}
