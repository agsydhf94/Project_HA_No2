using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class DashSkill : Skill
    {
        [Header("Dash")]
        public bool dashUnlocked;
        [SerializeField] private SkillTreeSlotUI dash_Unlock;

        [Header("Clone Dash")]
        public bool cloneAttackUnlocked;
        [SerializeField] private SkillTreeSlotUI cloneAttack_Unlock;

        

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
