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
        public ObjectPool objectPool;
        #endregion

        [Header("Sword Hit FX")]
        [SerializeField] private ParticleSystem mari_SwordHit;

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            fxManager = FXManager.Instance;
            objectPool = ObjectPool.Instance;

            objectPool.CreatePool("mari_SwordHit", mari_SwordHit, 2);
        }


        public void SwordFX()
        {
            Vector3 fxPosition = playerManager.playerCharacter.attackCheck.transform.position;
            Quaternion fxRotation = Quaternion.identity; // 필요 시 방향 지정

            fxManager.PlayEffect("mari_SwordHit", fxPosition, fxRotation, null, 0.5f);
        }

    }
}
