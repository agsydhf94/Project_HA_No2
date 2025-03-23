using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HA
{
    public class FXManager : MonoBehaviour
    {
        // ����Ʈ ������ ���� ����ü ����
        [System.Serializable]
        public struct FXPrefabEntry
        {
            public string key;
            public GameObject prefab;
            public int poolSize;
        }
        // �ν����Ϳ��� �پ��� ����Ʈ�� ����� �� �ִ� ����Ʈ
        public List<FXPrefabEntry> effectPrefabs;

        private ObjectPool objectPool;

        void Awake()
        {
            // �̱��� �ν��Ͻ� ����
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            // ObjectPool ������Ʈ �߰� �� ����Ʈ�� Ǯ ����
            objectPool = gameObject.AddComponent<ObjectPool>();
            foreach (var entry in effectPrefabs)
            {
                objectPool.InitializePool(entry.key, entry.prefab, entry.poolSize);
            }
        }

        // key�� �ش��ϴ� ����Ʈ�� �־��� ��ġ/ȸ������ ���
        public void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f)
        {
            GameObject fxObject = objectPool.GetFromPool(key);
            if (fxObject == null) return;
            // ��ġ �� �θ� ����
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

            // ����Ʈ ������Ʈ ���
            IFXPlayable fxPlayable = fxObject.GetComponent<IFXPlayable>();
            if (fxPlayable != null)
            {
                // �������̽� ����ü�� ��� �޼��� ȣ��
                fxPlayable.PlayEffect();
                // �Ϸ� �� �ڵ� ��ȯ �̺�Ʈ ����
                fxPlayable.OnEffectFinished += () => {
                    ReturnEffect(key, fxObject);
                };
            }
            else
            {
                // IFXPlayable�� �������� ���� ��� customDuration �� ��ȯ
                float returnTime = (customDuration > 0f) ? customDuration : 2f; // �⺻ 2�� �� �ʿ信 ���� ����
                StartCoroutine(AutoReturnAfter(key, fxObject, returnTime));
            }
        }

        // ���� �ð� �� ����Ʈ�� �ڵ� ��ȯ�ϴ� �ڷ�ƾ
        private IEnumerator AutoReturnAfter(string key, GameObject fxObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (fxObject.activeInHierarchy)
            {
                ReturnEffect(key, fxObject);
            }
        }

        // ����Ʈ�� Ǯ�� ��ȯ
        public void ReturnEffect(string key, GameObject fxObject)
        {
            // �θ� ���� ���� (�ٸ� ������Ʈ�� �ڽ����� �پ� �־��ٸ� Ǯ ���� ������ ����)
            fxObject.transform.parent = this.transform;
            objectPool.ReturnToPool(key, fxObject);
        }
    }
}
