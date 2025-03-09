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
        public ObjectPool objectPool;
        #endregion

        [Header("Sword Hit FX")]
        [SerializeField] private ParticleSystem mari_SwordHit;

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            objectPool = ObjectPool.Instance;

            objectPool.CreatePool("mari_SwordHit", mari_SwordHit, 2);
        }

        public async UniTask SwordFX()
        {
            var effectFx = objectPool.GetFromPool<ParticleSystem>("mari_SwordHit");
            effectFx.transform.position = playerManager.playerCharacter.attackCheck.transform.position;

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            objectPool.ReturnToPool("mari_SwordHit", effectFx);
        }
        
    }
}
