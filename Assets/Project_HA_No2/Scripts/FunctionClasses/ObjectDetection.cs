using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public static class ObjectDetection
    {
        public static List<Collider> GetObjectsBy<T>(Transform center, float radius, LayerMask layerMask = default) where T : class
        {
            // ������ �μ��� ���� ������, ���̾� ������� Ž��
            if (layerMask == default) layerMask = ~0;

            // ���ǿ� �´� �ݶ��̴��� �� ������ ���� ������ �ȵǱ� ������ �迭��� ����Ʈ�� ����
            List<Collider> result = new List<Collider>();
            Collider[] colliders = Physics.OverlapSphere(center.position, radius, layerMask);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<T>(out T _objectClass))
                {
                    result.Add(collider);
                }
            }

            return result;
        }
    }
}
