using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{
    public class DetectTargetOnScreen : SingletonBase<DetectTargetOnScreen>
    {

        [Header("Targeting Settings")]
        [SerializeField] private LayerMask targetLayer; // 감지할 레이어
        [SerializeField] private float maxDetectionDistance = 15f; // 감지 범위
        [SerializeField] private float detectionRadius = 5f; // 감지 반지름

        private List<TargetToStrike> validTargets = new List<TargetToStrike>();
        private int currentTargetIndex = 0;


        public void UpdateTargetList()
        {
            Debug.Log("UpdateTargetList 갱신중");

            List<TargetToStrike> detectedTargets = new List<TargetToStrike>();
            Vector3 rayStart = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
            Vector3 rayDirection = Camera.main.transform.forward;

            RaycastHit[] hits = Physics.SphereCastAll(rayStart, detectionRadius, rayDirection, maxDetectionDistance, targetLayer);


            float screenRadius = CalculateScreenRadius(rayStart, detectionRadius);

            foreach (RaycastHit hit in hits)
            {
                TargetToStrike target = hit.collider.GetComponent<TargetToStrike>();
                if (target == null) continue;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
                if (screenPos.z < 0) continue; // 카메라 뒤쪽 제거

                float distanceToCenter = Vector2.Distance(
                    new Vector2(Screen.width / 2, Screen.height / 2),
                    new Vector2(screenPos.x, screenPos.y)
                );

                if (distanceToCenter <= screenRadius)
                {
                    detectedTargets.Add(target);
                }
            }

            if (detectedTargets.SequenceEqual(validTargets)) return;

            validTargets = detectedTargets;

            if (validTargets.Count > 0)
            {
                validTargets.Sort(CompareByScreenCenter);
                currentTargetIndex = 0;
            }
        }

        private float CalculateScreenRadius(Vector3 worldPosition, float radius)
        {
            Vector3 worldRadiusPoint = worldPosition + Camera.main.transform.right * radius;
            return Vector2.Distance(Camera.main.WorldToScreenPoint(worldPosition), Camera.main.WorldToScreenPoint(worldRadiusPoint));
        }

        private int CompareByScreenCenter(TargetToStrike a, TargetToStrike b)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            float distA = Vector2.Distance(Camera.main.WorldToScreenPoint(a.transform.position), screenCenter);
            float distB = Vector2.Distance(Camera.main.WorldToScreenPoint(b.transform.position), screenCenter);
            return distA.CompareTo(distB);
        }

        public TargetToStrike GetCurrentTarget()
        {
            if (validTargets.Count == 0 || currentTargetIndex >= validTargets.Count)
                return null;

            return validTargets[currentTargetIndex];
        }

    }
}
