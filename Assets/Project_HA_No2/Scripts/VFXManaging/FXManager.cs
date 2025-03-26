using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HA
{
    public class FXManager : SingletonBase<FXManager>
    {
        private IFXFactory fxFactory;

        public override void Awake()
        {
            fxFactory = new FXFactory(ObjectPool.Instance); // 필요 시 교체 가능
        }

        public async void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f)
        {
            Component fxComponent = fxFactory.CreateFX(key);
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

            IFXPlayable fxPlayable = fxObject.GetComponent<IFXPlayable>();
            if (fxPlayable != null)
            {
                fxPlayable.PlayEffect();
                fxPlayable.OnEffectFinished += () => ReturnEffect(key, fxComponent);
            }
            else
            {
                float returnTime = (customDuration > 0f) ? customDuration : 2f;
                AutoReturnAfter(key, fxComponent, returnTime).Forget();
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
