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
            objectPool = ObjectPool.Instance;

            objectPool.CreatePool("mari_SwordHit", mari_SwordHit, 2);
        }

        // 이 엔티티의 위치를 기준으로 이펙트를 재생
        public void PlayEffect(string key, Vector3 offset, Quaternion rotation, float customDuration = -1f)
        {
            Vector3 worldPos = transform.position + offset;
            fxManager.PlayEffect(key, worldPos, rotation, null, customDuration);
        }

        // 이 엔티티에 이펙트를 붙여서 재생 (예: 지속되는 버프 효과 등)
        public void PlayAttachedEffect(string key, Vector3 localPosition, Quaternion rotation, float customDuration = -1f)
        {
            fxManager.PlayEffect(key, localPosition, rotation, this.transform, customDuration);
        }


        public void SwordFX()
        {
            Vector3 fxPosition = playerManager.playerCharacter.attackCheck.transform.position;
            Quaternion fxRotation = Quaternion.identity; // 필요 시 방향 지정

            fxManager.PlayEffect("mari_SwordHit", fxPosition, fxRotation, null, 0.5f);
        }

    }
}
