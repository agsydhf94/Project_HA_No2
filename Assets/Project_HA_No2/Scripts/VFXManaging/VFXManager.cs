using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HA
{
    public class VFXManager : SingletonBase<VFXManager>, IVFXPlayable
    {
        private IVFXFactory vfxFactory;
        public event Action OnEffectFinished;

        public override void Awake()
        {
            vfxFactory = new VFXFactory(ObjectPool.Instance); // 필요 시 교체 가능
        }

        public async void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f)
        {
            Component fxComponent = vfxFactory.LoadFX(key);
            if (fxComponent == null) return;

            GameObject fxObject = fxComponent.gameObject;

            if (parent != null)
            {
                fxObject.transform.SetParent(parent);
                fxObject.transform.localPosition = position;
            }
            else
            {
                fxObject.transform.SetParent(null);
                fxObject.transform.position = position;
            }

            fxObject.transform.rotation = rotation;
            fxObject.SetActive(true);

            AutoReturnAfter(key, fxComponent, customDuration).Forget();

            foreach (var comp in fxObject.GetComponents<MonoBehaviour>())
            {
                if (comp is IVFXPlayable fxPlayable)
                {
                    fxPlayable.PlayEffect(key, position, rotation, parent, customDuration);
                    fxPlayable.OnEffectFinished += () => ReturnEffect(key, fxComponent);
                    return;
                }
            }
        }

        private async UniTask AutoReturnAfter(string key, Component fxComponent, float seconds)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(seconds));
            if (fxComponent != null && fxComponent.gameObject.activeInHierarchy)
            {
                ReturnEffect(key, fxComponent);
            }
        }

        public void ReturnEffect(string key, Component fxComponent)
        {
            fxComponent.transform.SetParent(this.transform);
            ObjectPool.Instance.ReturnToPool(key, fxComponent);
        }
    }
}
