using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerAnimationTrigger : MonoBehaviour
    {
        private PlayerCharacter playerCharacter => GetComponent<PlayerCharacter>();

        private void AnimationTrigger()
        {
            playerCharacter.AnimationTrigger();
        }
    }
}
