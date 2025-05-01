using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class CounterAttackSkill : Skill
    {
        [Header("Counter Attack")]
        [SerializeField] private SkillTreeSlotUI counterAttack_Unlock;
        public bool counterAttackUnlocked { get; private set; }

        [Header("Restore")]
        [SerializeField] private SkillTreeSlotUI restore_Unlock;
        [Range(0f, 1f)]
        [SerializeField] private float restoreHealthPercent;
        public bool restoreUnlocked { get; private set; }

        [Header("Attack with clones")]
        [SerializeField] private SkillTreeSlotUI attackWithClones_Unlock;
        public bool attackWithClonesUnlocked { get; private set; }

        public override void UseSkill()
        {
            base.UseSkill();      
        }

        protected override void Start()
        {
            base.Start();

            counterAttack_Unlock.GetComponent<Button>().onClick.AddListener(UnlockCounterAttack);
            restore_Unlock.GetComponent<Button>().onClick.AddListener(UnlockRestore);
            attackWithClones_Unlock.GetComponent<Button>().onClick.AddListener(UnlockAttackWithClones);
        }

        private void UnlockCounterAttack()
        {
            if(counterAttack_Unlock.unlocked)
                counterAttackUnlocked = true;
        }

        private void UnlockRestore()
        {
            if(restore_Unlock.unlocked)
                restoreUnlocked = true;
        }

        private void UnlockAttackWithClones()
        {
            if(attackWithClones_Unlock.unlocked)
                attackWithClonesUnlocked = true;
        }


        public void MakeCloneOnCounterAttack(Transform respawnTransform)
        {
            if (attackWithClonesUnlocked)
                skillManager.cloneSkill.CreateCloneWithDelay(respawnTransform);
        }

        public void RestoreHealth()
        {
            if (restoreUnlocked)
            {
                int restoreAmount = Mathf.RoundToInt(playerCharacter.characterStats.GetMaxHealthValue() * restoreHealthPercent);
                playerCharacter.characterStats.IncreaseHealthBy(restoreAmount);
            }
        }
    }
}
