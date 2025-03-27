using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class GameManager : MonoBehaviour
    {
        private PlayerManager playerManager;

        private void Awake()
        {
            playerManager = PlayerManager.Instance;

            PlayerVFX_Injection();
        }

        private void PlayerVFX_Injection()
        {
            EntityVFX entityVFX = playerManager.playerCharacter.GetComponent<EntityVFX>();
            entityVFX.Initialize(VFXManager.Instance);
        }
    }
}
