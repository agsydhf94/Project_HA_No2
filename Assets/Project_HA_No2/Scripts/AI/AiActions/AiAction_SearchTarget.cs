using UnityEngine;
using System;

namespace HA
{
    public class AiAction_SearchTarget : IAiAction
    {
        private readonly Transform self;
        private readonly Camera enemyCamera;
        private readonly LayerMask targetLayerMask;
        private readonly LayerMask obstacleLayerMask;
        private readonly Func<float> getViewDistance;
        private readonly Action<Transform> setTarget;
        private readonly Action<string> changeState;

        private Plane[] cameraFrustum;

        public AiAction_SearchTarget(
            Transform self,
            Camera enemyCamera,
            LayerMask targetLayerMask,
            LayerMask obstacleLayerMask,
            Func<float> getViewDistance,
            Action<Transform> setTarget,
            Action<string> changeState)
        {
            this.self = self;
            this.enemyCamera = enemyCamera;
            this.targetLayerMask = targetLayerMask;
            this.obstacleLayerMask = obstacleLayerMask;
            this.getViewDistance = getViewDistance;
            this.setTarget = setTarget;
            this.changeState = changeState;
        }

        public void OnEnter()
        {
            // 필요 시 초기화
        }

        public void OnExit() { }

        public void OnUpdate()
        {
            var target = FindTargetByLayer();
            if (target != null)
            {
                setTarget(target);
                changeState("Chase");
            }
        }

        private Transform FindTargetByLayer()
        {
            cameraFrustum = GeometryUtility.CalculateFrustumPlanes(enemyCamera);
            float radius = getViewDistance();
            Collider[] hits = Physics.OverlapSphere(self.position, radius, targetLayerMask);

            Transform closest = null;
            float closestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                Bounds bounds = hit.bounds;
                if (!IsInCameraView(bounds)) continue;
                if (!HasLineOfSight(hit.transform)) continue;

                float dist = Vector3.Distance(self.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = hit.transform;
                }
            }

            if (closest != null)
                Debug.Log($"[SearchTarget] Target acquired: {closest.name}");

            return closest;
        }

        private bool IsInCameraView(Bounds bounds)
        {
            return GeometryUtility.TestPlanesAABB(cameraFrustum, bounds);
        }

        private bool HasLineOfSight(Transform target)
        {
            Vector3 direction = (target.position - self.position).normalized;
            float distance = Vector3.Distance(self.position, target.position);

            if (Physics.Raycast(self.position, direction, out RaycastHit hit, distance, obstacleLayerMask))
            {
                return hit.transform == target;
            }

            return true; // 시야 방해물이 없을 경우
        }
    }
}
