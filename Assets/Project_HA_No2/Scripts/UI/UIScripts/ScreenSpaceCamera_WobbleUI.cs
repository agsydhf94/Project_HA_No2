using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ScreenSpaceCamera_WobbleUI : MonoBehaviour
    {
        [Header("흔들림 기준 대상 (예: Player)")]
        public Transform followTarget;

        [Header("UI 기준 위치 (anchoredPosition 기준)")]
        public Vector2 baseAnchoredPosition = new Vector2(0f, 100f);

        [Header("흔들림 조정")]
        public float wobbleStrength = 10f;         // 흔들림 세기 (픽셀 단위)
        public float wobbleSmoothing = 5f;         // 흔들림 부드럽기 정도

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

            // 처음에 기준 위치 고정
            uiRectTransform.anchoredPosition = baseAnchoredPosition;
        }

        void Update()
        {
            if (followTarget == null) return;

            // 실제 이동 속도 측정
            playerVelocity = (followTarget.position - previousTargetPosition) / Time.deltaTime;
            previousTargetPosition = followTarget.position;
        }

        void LateUpdate()
        {
            // 흔들림을 X축 + Y축 모두 반영
            Vector2 targetWobble = new Vector2(-playerVelocity.x, -playerVelocity.y) * wobbleStrength;

            // 흔들림을 부드럽게 반영
            currentWobbleOffset = Vector2.Lerp(currentWobbleOffset, targetWobble, Time.deltaTime * wobbleSmoothing);

            // 기준 위치 + 흔들림 적용
            uiRectTransform.anchoredPosition = baseAnchoredPosition + currentWobbleOffset;
        }
    }
}

