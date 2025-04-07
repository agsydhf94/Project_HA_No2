using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        private Transform mainCamera;
        private Vector3 originalScale;

        [Header("거리 기반 크기 조절")]
        [SerializeField] private float minScale = 1f;
        [SerializeField] private float maxScale = 1.5f;
        [SerializeField] private float scaleDistanceFactor = 5f;

        private void Start()
        {
            mainCamera = Camera.main.transform;
            originalScale = transform.localScale;
        }

        private void LateUpdate()
        {
            // 카메라 바라보기
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);

            // 거리 기반 스케일 조절
            float distance = Vector3.Distance(mainCamera.position, transform.position);
            float scaleMultiplier = Mathf.Clamp(distance / scaleDistanceFactor, minScale, maxScale);

            transform.localScale = originalScale * scaleMultiplier;
        }
    }
}
