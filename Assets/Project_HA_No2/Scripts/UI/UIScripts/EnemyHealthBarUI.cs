using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        private Transform mainCamera;
        private Vector3 originalScale;

        [Header("�Ÿ� ��� ũ�� ����")]
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
            // ī�޶� �ٶ󺸱�
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);

            // �Ÿ� ��� ������ ����
            float distance = Vector3.Distance(mainCamera.position, transform.position);
            float scaleMultiplier = Mathf.Clamp(distance / scaleDistanceFactor, minScale, maxScale);

            transform.localScale = originalScale * scaleMultiplier;
        }
    }
}
