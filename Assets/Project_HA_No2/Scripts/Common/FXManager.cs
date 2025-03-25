using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HA
{
    public class FXManager : SingletonBase<FXManager>
    {
        public override void Awake()
        {
            // 초기화 로직이 필요 없다면 생략 가능
        }

        /// <summary>
        /// 외부에서 받은 이펙트 오브젝트를 재생
        /// </summary>
        public async UniTask PlayEffect(Component fxComponent, string key = "", Vector3? position = null, Quaternion? rotation = null, Transform parent = null, float customDuration = -1f, Action<string, Component> onReturn = null)
        {
            if (fxComponent == null) return;

            GameObject fxObject = fxComponent.gameObject;

            // 위치 및 부모 설정
            fxObject.transform.SetParent(parent);
            fxObject.transform.localPosition = position ?? Vector3.zero;
            fxObject.transform.localRotation = rotation ?? Quaternion.identity;

            fxObject.SetActive(true);

            IFXPlayable fxPlayable = fxObject.GetComponent<IFXPlayable>();
            if (fxPlayable != null)
            {
                fxPlayable.PlayEffect();

                fxPlayable.OnEffectFinished += () =>
                {
                    onReturn?.Invoke(key, fxComponent);
                };
            }
            else
            {
                float returnTime = (customDuration > 0f) ? customDuration : 2f;
                await UniTask.Delay(TimeSpan.FromSeconds(returnTime));

                if (fxObject.activeInHierarchy)
                {
                    onReturn?.Invoke(key, fxComponent);
                }
            }
        }
    }
}
