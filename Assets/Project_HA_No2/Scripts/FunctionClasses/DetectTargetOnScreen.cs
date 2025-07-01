using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Singleton class responsible for detecting potential target objects on screen within a given distance and radius.
    /// Provides functionality for updating the target list, cycling through targets, and determining the current target.
    /// </summary>
    public class DetectTargetOnScreen : SingletonBase<DetectTargetOnScreen>
    {

        [Header("Targeting Settings")]
        [SerializeField] private float maxDetectionDistance = 1000f;
        [SerializeField] private float detectionRadius = 100f;
        private Camera mainCamera;

        private List<TargetToStrike> validTargets = new List<TargetToStrike>();
        private int currentTargetIndex = 0;

        public void SetCamera(Camera camera)
        {
            mainCamera = Camera.main;
        }


        /// <summary>
        /// Updates the list of valid targets that are within detection radius and visible on screen.
        /// Filters targets based on screen space position and sorts them by distance from screen center.
        /// </summary>
        public void UpdateTargetList()
        {
            List<TargetToStrike> detectedTargets = new List<TargetToStrike>();

            Vector3 rayStart = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
            Vector3 rayDirection = Camera.main.transform.forward;

            RaycastHit[] hits = Physics.SphereCastAll(rayStart, detectionRadius, rayDirection, maxDetectionDistance);

            float screenRadius = CalculateScreenRadius(rayStart, detectionRadius);

            foreach (RaycastHit hit in hits)
            {
                TargetToStrike target = hit.collider.GetComponent<TargetToStrike>();
                if (target == null) continue;

                Transform targetPoint = target.rb.transform;
                if (targetPoint == null) continue; 

                Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPoint.position);

                if (screenPos.z < 0) continue;  // Behind camera

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


        /// <summary>
        /// Converts a world-space radius to its corresponding screen-space radius (in pixels).
        /// </summary>
        private float CalculateScreenRadius(Vector3 worldPosition, float radius)
        {
            Vector3 worldRadiusPoint = worldPosition + Camera.main.transform.right * radius;
            return Vector2.Distance(
                Camera.main.WorldToScreenPoint(worldPosition),
                Camera.main.WorldToScreenPoint(worldRadiusPoint)
            );
        }


        /// <summary>
        /// Sort comparison method to order targets based on distance from the center of the screen.
        /// </summary>
        private int CompareByScreenCenter(TargetToStrike a, TargetToStrike b)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            float distA = Vector2.Distance(Camera.main.WorldToScreenPoint(a.rb.position), screenCenter);
            float distB = Vector2.Distance(Camera.main.WorldToScreenPoint(b.rb.position), screenCenter);
            return distA.CompareTo(distB);
        }


        /// <summary>
        /// Handles input from mouse scroll wheel to cycle through detected targets.
        /// </summary>
        public void HandleScrollInput()
        {
            if (validTargets.Count <= 1) return;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                currentTargetIndex = (currentTargetIndex + 1) % validTargets.Count;
            }
            else if (scroll < 0f)
            {
                currentTargetIndex = (currentTargetIndex - 1 + validTargets.Count) % validTargets.Count;
            }
        }


        /// <summary>
        /// Returns the currently selected target from the valid target list.
        /// </summary>
        public TargetToStrike GetCurrentTarget()
        {
            if (validTargets.Count == 0 || currentTargetIndex >= validTargets.Count)
                return null;

            return validTargets[currentTargetIndex];
        }


        /// <summary>
        /// Returns the full list of currently detected valid targets.
        /// </summary>
        public List<TargetToStrike> GetAllTargets() => validTargets;

    }
}
