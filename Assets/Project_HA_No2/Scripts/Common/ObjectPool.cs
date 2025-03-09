using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ObjectPool : SingletonBase<ObjectPool>
    {
        public Dictionary<string, Queue<Component>> objectPools = new Dictionary<string, Queue<Component>>();

        // ������Ʈ Ǯ ���� �� �ʱ�ȭ
        public void CreatePool<T>(string poolKey, T prefab, int initialSize) where T : Component
        {
            if (!objectPools.ContainsKey(poolKey))
            {
                objectPools[poolKey] = new Queue<Component>();

                for (int i = 0; i < initialSize; i++)
                {
                    T newObject = Instantiate(prefab);
                    newObject.gameObject.SetActive(false);
                    objectPools[poolKey].Enqueue(newObject);
                }
            }
        }

        // ������Ʈ Ǯ���� ��������
        public T GetFromPool<T>(string poolKey) where T : Component
        {
            if (objectPools.ContainsKey(poolKey) && objectPools[poolKey].Count > 0)
            {
                T objectToReuse = objectPools[poolKey].Dequeue() as T;
                objectToReuse.gameObject.SetActive(true);
                return objectToReuse;
            }
            else
            {
                // Ǯ�� ������Ʈ�� ������ null ��ȯ (�ʿ�� ���ο� ������Ʈ�� �����ϴ� ���� �߰� ����)
                return null;
            }
        }

        // ������Ʈ�� Ǯ�� ��ȯ
        public void ReturnToPool<T>(string poolKey, T objectToReturn) where T : Component
        {
            if (objectPools.ContainsKey(poolKey))
            {
                objectToReturn.gameObject.SetActive(false);
                objectPools[poolKey].Enqueue(objectToReturn);
            }
        }
    }
}
