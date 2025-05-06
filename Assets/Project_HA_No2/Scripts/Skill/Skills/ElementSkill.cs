using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class ElementSkill : Skill
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private float elementDuration;
        private GameObject currentElement;
        private ElementSkillController currentController;

        [Header("Element Simple")]
        [SerializeField] private SkillTreeSlotUI element_Unlock;
        public bool elementUnlocked { get; private set; }

        [Header("Clone Instead Element")]
        [SerializeField] private SkillTreeSlotUI cloneInstead_Unlock;
        [SerializeField] public bool cloneInsteadUnlocked { get; private set; }

        [Header("Explosive Element")]
        [SerializeField] private SkillTreeSlotUI explodeElement_Unlock;
        [SerializeField] public bool canExplode { get; private set; }
        [SerializeField] private float explodeRadius;
        [SerializeField] private LayerMask explodeLayer;

        [Header("Moving Element")]
        [SerializeField] private SkillTreeSlotUI movingElement_Unlock;
        [SerializeField] public bool canMoveToEnemy { get; private set; }
        [SerializeField] private float moveSpeed;

        [Header("Element Stacking Informations")]
        [SerializeField] private SkillTreeSlotUI stackingElement_Unlock;
        [SerializeField] public bool canUseMultiStacks { get; private set; }
        [SerializeField] private int amountOfStacks;
        [SerializeField] private float multiStackCooldown;
        [SerializeField] private float timeWindow;
        [SerializeField] private List<GameObject> elementsLeft = new List<GameObject>();


        protected override void Start()
        {
            base.Start();

            element_Unlock.GetComponent<Button>().onClick.AddListener(UnlockElement);
            cloneInstead_Unlock.GetComponent<Button>().onClick.AddListener(UnlockCloneInsteadOfElement);
            explodeElement_Unlock.GetComponent<Button>().onClick.AddListener(UnlockExplodeElement);
            movingElement_Unlock.GetComponent<Button>().onClick.AddListener(UnlockMovingElement);
            stackingElement_Unlock.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.E) && elementUnlocked)
            {
                CanUseSkill();
            }
        }
        #region Unlock Skill

        protected override void CheckUnlock()
        {
            UnlockElement();
            UnlockCloneInsteadOfElement();
            UnlockExplodeElement();
            UnlockMovingElement();
            UnlockMultiStack();
        }

        private void UnlockElement()
        {
            if (element_Unlock.unlocked)
                elementUnlocked = true;
        }

        private void UnlockCloneInsteadOfElement()
        {
            if(cloneInstead_Unlock.unlocked)
                cloneInsteadUnlocked = true;
        }

        private void UnlockExplodeElement()
        {
            if(explodeElement_Unlock.unlocked)
                canExplode = true;
        }

        private void UnlockMovingElement()
        {
            if(movingElement_Unlock.unlocked)
                canMoveToEnemy = true;
        }

        private void UnlockMultiStack()
        {
            if(stackingElement_Unlock.unlocked)
                canUseMultiStacks = true;
        }

        #endregion


        public override bool CanUseSkill()
        {
            return base.CanUseSkill();
        }

        public override void UseSkill()
        {
            base.UseSkill();

            if (CanUseMultiElements())
                return;

            if(currentElement == null)
            {
                CreateElement();

                if (canExplode)
                {
                    currentController.SetupExplode(explodeRadius, explodeLayer);
                }

            }
            else
            {                
                if(canMoveToEnemy)
                {
                    return;
                }

                Vector3 playerPosition = playerCharacter.transform.position;
                
                playerCharacter.GetComponent<CharacterController>().enabled = false;
                playerCharacter.transform.position = currentElement.transform.position - new Vector3(0f, 1f, 0f);
                playerCharacter.GetComponent<CharacterController>().enabled = true;
                currentElement.transform.position = playerPosition;

                if (cloneInsteadUnlocked)
                {
                    skillManager.cloneSkill.CreateClone(currentElement.transform, Vector3.zero);
                    Destroy(currentElement);
                }
                else
                {
                    currentController.FinishElement();
                }
            }
        }

        public void CreateElement()
        {
            currentElement = Instantiate(elementPrefab);
            currentElement.transform.position = playerCharacter.transform.position + new Vector3(0f, 1f, 0f);

            currentController = currentElement.GetComponent<ElementSkillController>();

            var closestEnemy = FindClosestEnemy.GetClosestEnemy(currentElement.transform);
            currentController.SetupElement(elementDuration, canExplode, canMoveToEnemy, moveSpeed, closestEnemy);

            
            
        }

        public void CurrentElementChooseRandomTarget()
        {
            currentController.ChooseRandomEnemy();
        }

        private void RestoreElement()
        {
            int amountToAdd = amountOfStacks - elementsLeft.Count;

            for(int i = 0; i < amountToAdd; i++)
            {
                elementsLeft.Add(elementPrefab);
            }
        }

        private bool CanUseMultiElements()
        {
            if(canUseMultiStacks)
            {
                if(elementsLeft.Count > 0)
                {
                    if(elementsLeft.Count == amountOfStacks)
                    {
                        Invoke("ResetAbility", timeWindow);
                    }

                    cooldown = 0f;
                    GameObject elementToSpawn = elementsLeft[elementsLeft.Count - 1];
                    GameObject newElement = Instantiate(elementToSpawn, playerCharacter.transform.position, Quaternion.identity);

                    elementsLeft.Remove(elementToSpawn);
                    var closestEnemy = FindClosestEnemy.GetClosestEnemy(newElement.transform);
                    newElement.GetComponent<ElementSkillController>().SetupElement(elementDuration, canExplode, canMoveToEnemy, moveSpeed, closestEnemy);

                    if(elementsLeft.Count <= 0)
                    {
                        // 쿨다운 후 다시 원소 스킬을 사용할 수 있게 채움
                        cooldown = multiStackCooldown;
                        RestoreElement();
                    }
                    
                    return true;

                }

                
            }
            
            return false;
        }

        private void ResetAbility()
        {
            if (cooldownTimer > 0)
                return;

            cooldownTimer = multiStackCooldown;
            RestoreElement();
        }

        
    }
}
