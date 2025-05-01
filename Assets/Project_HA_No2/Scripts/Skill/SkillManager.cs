using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class SkillManager : SingletonBase<SkillManager>
    {
        public DashSkill dashSkill;
        public CloneSkill cloneSkill;
        public BallThrowSkill ballThrowSkill;
        public BlackHoleSkill blackHoleSkill;
        public ElementSkill elementSkill;
        public CounterAttackSkill counterAttackSkill;
        public DodgeSkill dodgeSkill;




        private void Start()
        {
            Initialize_BallthrowSkill();
        }

        private void Initialize_BallthrowSkill()
        {
            ballThrowSkill.InitializeSpawner(ObjectManager.Instance);
        }
    }
}
