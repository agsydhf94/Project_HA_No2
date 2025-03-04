using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        private PlayerCharacter playerCharacter => GetComponent<PlayerCharacter>();

        private void AnimationTrigger_On()
        {
            playerCharacter.AnimationTrigger();
        }
    }
}
