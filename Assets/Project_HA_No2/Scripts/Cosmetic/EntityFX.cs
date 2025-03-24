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

        // �� ��ƼƼ�� ��ġ�� �������� ����Ʈ�� ���
        public void PlayEffect(string key, Vector3 offset, Quaternion rotation, float customDuration = -1f)
        {
            Vector3 worldPos = transform.position + offset;
            fxManager.PlayEffect(key, worldPos, rotation, null, customDuration);
        }

        // �� ��ƼƼ�� ����Ʈ�� �ٿ��� ��� (��: ���ӵǴ� ���� ȿ�� ��)
        public void PlayAttachedEffect(string key, Vector3 localPosition, Quaternion rotation, float customDuration = -1f)
        {
            fxManager.PlayEffect(key, localPosition, rotation, this.transform, customDuration);
        }


        public void SwordFX()
        {
            Vector3 fxPosition = playerManager.playerCharacter.attackCheck.transform.position;
            Quaternion fxRotation = Quaternion.identity; // �ʿ� �� ���� ����

            fxManager.PlayEffect("mari_SwordHit", fxPosition, fxRotation, null, 0.5f);
        }

    }
}
