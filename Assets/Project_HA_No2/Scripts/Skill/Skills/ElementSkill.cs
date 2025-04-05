using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ElementSkill : Skill
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private float elementDuration;
        private GameObject currentElement;

        [Header("Moving Element")]
        [SerializeField] private bool canMoveToEnemy;
        [SerializeField] private float moveSpeed;

        [Header("Explosive Element")]
        [SerializeField] private bool canExplode;
        [SerializeField] private float explodeRadius;
        [SerializeField] private LayerMask explodeLayer;

        public override bool CanUseSkill()
        {
            return base.CanUseSkill();
        }

        public override void UseSkill()
        {
            base.UseSkill();

            if(currentElement == null)
            {
                currentElement = Instantiate(elementPrefab);
                currentElement.transform.position = playerCharacter.transform.position + new Vector3(0f, 1f, 0f);

                ElementSkillController currentElementController = currentElement.GetComponent<ElementSkillController>();
                if(canExplode)
                {
                    currentElementController.SetupElement(elementDuration, canExplode, canMoveToEnemy, moveSpeed);
                    currentElementController.SetupExplode(explodeRadius, explodeLayer);
                }
                else
                {
                    currentElementController.SetupElement(elementDuration, canExplode, canMoveToEnemy, moveSpeed);
                }
                
            }
            else
            {
                playerCharacter.GetComponent<CharacterController>().enabled = false;
                playerCharacter.transform.position = currentElement.transform.position - new Vector3(0f, 1f, 0f);
                playerCharacter.GetComponent<CharacterController>().enabled = true;
                currentElement.GetComponent<ElementSkillController>().FinishElement();
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if(Input.GetKeyDown(KeyCode.E))
            {
                CanUseSkill(); 
            }
        }
    }
}
