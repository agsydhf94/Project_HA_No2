using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace HA
{
    public class EntityVFX : MonoBehaviour, IVFXInjection
    {
        #region Components
        public PlayerManager playerManager;
        public IVFXPlayable ivfxPlayable;
        #endregion

        

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
        }

        public void Initialize(IVFXPlayable injectedVFXPlayer)
        {
            ivfxPlayable = injectedVFXPlayer;
        }


        public void SwordHitVFX()
        {
            Vector3 fxPosition = playerManager.playerCharacter.attackCheck.transform.position;
            Quaternion fxRotation = Quaternion.identity; // 필요 시 방향 지정

            ivfxPlayable.PlayEffect("mari_SwordHit", fxPosition, fxRotation, null, 0.5f);
        }

    }
}
