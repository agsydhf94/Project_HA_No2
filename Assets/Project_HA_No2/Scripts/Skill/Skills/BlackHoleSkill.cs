using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class BlackHoleSkill : Skill
    {
        [SerializeField] private SkillTreeSlotUI blackHole_Unlock;
        public bool blackHoleUnlocked;

        [SerializeField] private int amountOfAttacks;
        [SerializeField] private float cloneCoolTime;
        [SerializeField] private float blackHoleDuration;
        [SerializeField] private GameObject blackHoldPrefab;
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;

        private BlackHoleSkillController currentBlackHole;

        public override bool CanUseSkill()
        {
            return base.CanUseSkill();
        }

        public override void UseSkill()
        {
            base.UseSkill();

            currentBlackHole = Instantiate(blackHoldPrefab).GetComponent<BlackHoleSkillController>();
            currentBlackHole.transform.SetParent(playerCharacter.transform);
            currentBlackHole.transform.localPosition = Vector3.zero;

            var controller = currentBlackHole.GetComponent<BlackHoleSkillController>();
            controller.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCoolTime, blackHoleDuration);
        }

        protected override void Start()
        {
            base.Start();

            blackHole_Unlock.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void CheckUnlock()
        {
            UnlockBlackHole();
        }

        private void UnlockBlackHole()
        {
            if(blackHole_Unlock.unlocked)
            {
                blackHoleUnlocked = true;
            }
        }

        public bool BlackHoleFinished()
        {
            if (!currentBlackHole)
                return false;

            if(currentBlackHole.playerCanExitState)
            {
                currentBlackHole = null;
                return true;
            }
            return false;
        }
    }
}
