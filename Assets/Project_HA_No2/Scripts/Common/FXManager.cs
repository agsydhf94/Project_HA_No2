using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HA
{
    public class FXManager : MonoBehaviour
    {
        // 이펙트 프리팹 설정 구조체 정의
        [System.Serializable]
        public struct FXPrefabEntry
        {
            public string key;
            public GameObject prefab;
            public int poolSize;
        }
        // 인스펙터에서 다양한 이펙트를 등록할 수 있는 리스트
        public List<FXPrefabEntry> effectPrefabs;

        private ObjectPool objectPool;

        void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            // ObjectPool 컴포넌트 추가 및 이펙트들 풀 세팅
            objectPool = gameObject.AddComponent<ObjectPool>();
            foreach (var entry in effectPrefabs)
            {
                objectPool.InitializePool(entry.key, entry.prefab, entry.poolSize);
            }
        }

        // key에 해당하는 이펙트를 주어진 위치/회전에서 재생
        public void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f)
        {
            GameObject fxObject = objectPool.GetFromPool(key);
            if (fxObject == null) return;
            // 위치 및 부모 지정
            if (parent != null)
            {
                fxObject.transform.parent = parent;
                fxObject.transform.localPosition = position;
            }
            else
            {
                fxObject.transform.position = position;
            }
            fxObject.transform.rotation = rotation;
            fxObject.SetActive(true);

            // 이펙트 컴포넌트 재생
            IFXPlayable fxPlayable = fxObject.GetComponent<IFXPlayable>();
            if (fxPlayable != null)
            {
                // 인터페이스 구현체의 재생 메서드 호출
                fxPlayable.PlayEffect();
                // 완료 시 자동 반환 이벤트 연결
                fxPlayable.OnEffectFinished += () => {
                    ReturnEffect(key, fxObject);
                };
            }
            else
            {
                // IFXPlayable을 구현하지 않은 경우 customDuration 후 반환
                float returnTime = (customDuration > 0f) ? customDuration : 2f; // 기본 2초 등 필요에 따라 조절
                StartCoroutine(AutoReturnAfter(key, fxObject, returnTime));
            }
        }

        // 일정 시간 후 이펙트를 자동 반환하는 코루틴
        private IEnumerator AutoReturnAfter(string key, GameObject fxObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (fxObject.activeInHierarchy)
            {
                ReturnEffect(key, fxObject);
            }
        }

        // 이펙트를 풀에 반환
        public void ReturnEffect(string key, GameObject fxObject)
        {
            // 부모 연결 해제 (다른 오브젝트의 자식으로 붙어 있었다면 풀 관리 하위로 복귀)
            fxObject.transform.parent = this.transform;
            objectPool.ReturnToPool(key, fxObject);
        }
    }
}
