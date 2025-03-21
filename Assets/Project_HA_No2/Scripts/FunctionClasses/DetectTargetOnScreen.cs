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
        [SerializeField] private float maxDetectionDistance = 1000f;
        [SerializeField] private float detectionRadius = 100f;

        private List<ITargetable> validTargets = new List<ITargetable>();
        private int currentTargetIndex = 0;

        public void UpdateTargetList()
        {
            List<ITargetable> detectedTargets = new List<ITargetable>();

            Vector3 rayStart = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
            Vector3 rayDirection = Camera.main.transform.forward;

            // LayerMask 없이 모든 Collider 대상
            RaycastHit[] hits = Physics.SphereCastAll(rayStart, detectionRadius, rayDirection, maxDetectionDistance);

            float screenRadius = CalculateScreenRadius(rayStart, detectionRadius);

            foreach (RaycastHit hit in hits)
            {
                ITargetable target = hit.collider.GetComponent<ITargetable>();
                if (target == null) continue;

                Transform targetPoint = target.GetTargetPoint();
                if (targetPoint == null) continue; // null이면 감지 무시

                Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPoint.position);

                if (screenPos.z < 0) continue;

                float distanceToCenter = Vector2.Distance(
                    new Vector2(Screen.width / 2f, Screen.height / 2f),
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
            return Vector2.Distance(
                Camera.main.WorldToScreenPoint(worldPosition),
                Camera.main.WorldToScreenPoint(worldRadiusPoint)
            );
        }

        private int CompareByScreenCenter(ITargetable a, ITargetable b)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            float distA = Vector2.Distance(Camera.main.WorldToScreenPoint(a.GetTargetPoint().position), screenCenter);
            float distB = Vector2.Distance(Camera.main.WorldToScreenPoint(b.GetTargetPoint().position), screenCenter);
            return distA.CompareTo(distB);
        }

        public ITargetable GetCurrentTarget()
        {
            if (validTargets.Count == 0 || currentTargetIndex >= validTargets.Count)
                return null;

            return validTargets[currentTargetIndex];
        }

    }
}
