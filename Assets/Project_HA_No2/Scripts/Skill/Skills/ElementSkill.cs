using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ElementSkill : Skill
    {
        [SerializeField] private GameObject elementPrefab;
        private GameObject currentElement;

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
            }
            else
            {
                playerCharacter.transform.position = currentElement.transform.position - new Vector3(0f, 1f, 0f);
                Destroy(currentElement);
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
