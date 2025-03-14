using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace HA
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float cooldown;
        protected float cooldownTimer;

        protected virtual void Update()
        {
            cooldownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if(cooldownTimer < 0)
            {
                // Use Skill
                cooldownTimer = cooldown;
                return true;
            }

            Debug.Log("Skill Cooldown Please Wait");
            return false;
        }

        public virtual void UseSkill()
        {

        }
    }
}
