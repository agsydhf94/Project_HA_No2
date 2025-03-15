using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class SkillManager : SingletonBase<SkillManager>
    {
        public DashSkill dashSkill;

        public override void Awake()
        {
            
        }

        private void Start()
        {
            dashSkill = GetComponent<DashSkill>();
        }
    }
}
