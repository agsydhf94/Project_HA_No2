using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class SkillManager : SingletonBase<SkillManager>
    {
        public DashSkill dashSkill { get; private set; }
        public CloneSkill cloneSkill { get; private set; }

        public BallThrowSkill ballThrowSkill { get; private set; }

        public override void Awake()
        {
            
        }

        private void Start()
        {
            dashSkill = GetComponent<DashSkill>();
            cloneSkill = GetComponent<CloneSkill>();
            ballThrowSkill = GetComponent<BallThrowSkill>();
        }
    }
}
