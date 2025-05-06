using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class DodgeSkill : Skill
    {
        [Header("Dodge")]
        [SerializeField] private SkillTreeSlotUI dodge_Unlock;
        [SerializeField] private int evastionAmount;
        public bool dodgeUnlocked { get; private set; }

        [Header("Clone Dodge")]
        [SerializeField] private SkillTreeSlotUI clonedodge_Unlock;
        public bool clonedodgeUnlocked { get; private set; }

        protected override void Start()
        {
            base.Start();

            dodge_Unlock.GetComponent<Button>().onClick.AddListener(UnlockDodge);
            clonedodge_Unlock.GetComponent<Button>().onClick.AddListener(UnlockCloneDodge);
        }

        protected override void CheckUnlock()
        {
            UnlockDodge();
            UnlockCloneDodge();
        }   

        private void UnlockDodge()
        {
            if(dodge_Unlock.unlocked && !dodgeUnlocked)
            {
                playerCharacter.characterStats.evasion.AddModifier(evastionAmount);
                Inventory.Instance.UptateStatUI();

                dodgeUnlocked = true;
            }
                
        }

        private void UnlockCloneDodge()
        {
            if(clonedodge_Unlock.unlocked)
                clonedodgeUnlocked = true;
        }

        
        public void CreateCloneOnDodge()
        {
            if (clonedodgeUnlocked)
                skillManager.cloneSkill.CreateClone(playerCharacter.transform, Vector3.zero);
        }

    }
}
