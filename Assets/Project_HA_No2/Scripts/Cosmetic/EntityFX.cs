using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace HA
{
    public class EntityFX : MonoBehaviour
    {
        #region Components
        public PlayerManager playerManager;
        public FXManager fxManager;
        #endregion

        

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            fxManager = FXManager.Instance;
        }


        public void SwordHitVFX()
        {
            Vector3 fxPosition = playerManager.playerCharacter.attackCheck.transform.position;
            Quaternion fxRotation = Quaternion.identity; // �ʿ� �� ���� ����

            fxManager.PlayEffect("mari_SwordHit", fxPosition, fxRotation, null, 0.5f);
        }

    }
}
