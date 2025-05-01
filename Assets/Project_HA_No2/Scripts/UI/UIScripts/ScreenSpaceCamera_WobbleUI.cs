using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ScreenSpaceCamera_WobbleUI : MonoBehaviour
    {
        [Header("��鸲 ���� ��� (��: Player)")]
        public Transform followTarget;

        [Header("UI ���� ��ġ (anchoredPosition ����)")]
        public Vector2 baseAnchoredPosition = new Vector2(0f, 100f);

        [Header("��鸲 ����")]
        public float wobbleStrength = 10f;         // ��鸲 ���� (�ȼ� ����)
        public float wobbleSmoothing = 5f;         // ��鸲 �ε巴�� ����

        public RectTransform uiRectTransform;
        private Vector3 previousTargetPosition;
        private Vector3 playerVelocity;
        private Vector2 currentWobbleOffset;

        void Start()
        {

            if (followTarget == null)
                followTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (followTarget != null)
                previousTargetPosition = followTarget.position;

            // ó���� ���� ��ġ ����
            uiRectTransform.anchoredPosition = baseAnchoredPosition;
        }

        void Update()
        {
            if (followTarget == null) return;

            // ���� �̵� �ӵ� ����
            playerVelocity = (followTarget.position - previousTargetPosition) / Time.deltaTime;
            previousTargetPosition = followTarget.position;
        }

        void LateUpdate()
        {
            // ��鸲�� X�� + Y�� ��� �ݿ�
            Vector2 targetWobble = new Vector2(-playerVelocity.x, -playerVelocity.y) * wobbleStrength;

            // ��鸲�� �ε巴�� �ݿ�
            currentWobbleOffset = Vector2.Lerp(currentWobbleOffset, targetWobble, Time.deltaTime * wobbleSmoothing);

            // ���� ��ġ + ��鸲 ����
            uiRectTransform.anchoredPosition = baseAnchoredPosition + currentWobbleOffset;
        }
    }
}

