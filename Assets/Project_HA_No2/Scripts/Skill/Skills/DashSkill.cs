using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class DashSkill : Skill
    {
        [Header("Dash")]
        [SerializeField] private SkillTreeSlotUI dash_Unlock;
        public bool dashUnlocked { get; private set; }

        [Header("Clone Dash")]
        [SerializeField] private SkillTreeSlotUI cloneAttack_Unlock;
        public bool cloneAttackUnlocked { get; private set; }

        

        protected override void Start()
        {
            base.Start();

            dash_Unlock.GetComponent<Button>().onClick.AddListener(UnlockDash);
            cloneAttack_Unlock.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);            
        }

        public override void UseSkill()
        {
            base.UseSkill();
        }

        private void UnlockDash()
        {

            if (dash_Unlock.unlocked)
            {
                dashUnlocked = true;
            }
                
        }
            
        private void UnlockCloneAttack()
        {
            if(cloneAttack_Unlock.unlocked)
            {
                cloneAttackUnlocked = true;
            }
        }


        public void CloneAttack()
        {
            if (cloneAttackUnlocked)
            {
                skillManager.cloneSkill.CreateClone(playerCharacter.transform, Vector3.zero);
            }
        }


    }
}
