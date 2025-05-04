using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ShakingUI : MonoBehaviour
    {
        public Vector3 basePosition; // �⺻ ��ġ (ȭ��� ��ġ)
        public float shakeIntensity = 0.05f; // ��鸲 ����
        public float shakeSpeed = 2f; // ��鸲 �ӵ� ����

        private void Start()
        {
            // UI�� �⺻ ��ġ�� ȭ�� �������� ���� (�ʱ� ��ġ ����)
            basePosition = transform.position;
        }

        private void LateUpdate()
        {
            // Perlin Noise�� �̿��� ��鸲 ȿ��
            Vector3 shakeOffset = new Vector3(
                Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f,
                0f
            ) * shakeIntensity;

            // �⺻ ��ġ�� ��鸲�� ���Ͽ� ������ ��ġ���� ��¦ �����̵��� ����
            transform.position = basePosition + shakeOffset;
        }
    }
}
