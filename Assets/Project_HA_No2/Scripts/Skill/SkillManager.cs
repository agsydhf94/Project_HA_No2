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
        public BlackHoleSkill blackHoleSkill { get; private set; }
        public ElementSkill elementSkill { get; private set; }
        public CounterAttackSkill counterAttackSkill { get; private set; }




        private void Start()
        {
            dashSkill = GetComponent<DashSkill>();
            cloneSkill = GetComponent<CloneSkill>();
            ballThrowSkill = GetComponent<BallThrowSkill>();
            blackHoleSkill = GetComponent<BlackHoleSkill>();
            elementSkill = GetComponent<ElementSkill>();
            counterAttackSkill = GetComponent<CounterAttackSkill>();

            Initialize_BallthrowSkill();
        }

        private void Initialize_BallthrowSkill()
        {
            ballThrowSkill.InitializeSpawner(ObjectManager.Instance);
        }
    }
}
