using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public static class ObjectDetection
    {
        public static List<Collider> GetObjectsBy<T>(Transform center, float radius, LayerMask layerMask = default) where T : class
        {
            // 마지막 인수를 쓰지 않으면, 레이어 상관없이 탐색
            if (layerMask == default) layerMask = ~0;

            // 조건에 맞는 콜라이더가 몇 개인지 길이 예측이 안되기 때문에 배열대신 리스트로 선언
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
