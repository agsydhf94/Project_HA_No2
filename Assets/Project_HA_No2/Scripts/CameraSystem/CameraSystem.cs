using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace HA
{
    public enum CameraType
    {
        TPS_NotAim, TPS_Aim
    }

    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; } = null;

        // 각각의 VirtualCamera GameObject
        public CinemachineVirtualCamera tpsCamera_NotAim;
        public CinemachineVirtualCamera tpsCamera_Aim;

        public Vector3 AimingPoint { get; private set; }

        public LayerMask aimingLayerMask;

        private CameraType currentCameraType = CameraType.TPS_NotAim;
        private bool isZoom = false;

        private void Awake()
        {
            Instance = this;

            // Start 시점엔 NotAim이 Priority가 높도록
            tpsCamera_NotAim.Priority = 11;
            tpsCamera_Aim.Priority = 10;
        }


        public void SetCameraFollowTarget(Transform target)
        {
            tpsCamera_NotAim.Follow = target;
            tpsCamera_NotAim.LookAt = target;

            tpsCamera_Aim.Follow = target;
            tpsCamera_Aim.LookAt = target;
        }

        public void ChangeCameraType(CameraType newType)
        {
            if (currentCameraType == newType) return;
            currentCameraType = newType;

            switch (newType)
            {
                case CameraType.TPS_NotAim:
                    tpsCamera_NotAim.Priority = 11;
                    tpsCamera_Aim.Priority = 10;
                    break;

                case CameraType.TPS_Aim:
                    tpsCamera_NotAim.Priority = 10;
                    tpsCamera_Aim.Priority = 11;
                    break;
            }
        }

        public void ZoomInToAim()
        {
            ChangeCameraType(CameraType.TPS_Aim);
        }

        public void ZoomOutToDefault()
        {
            ChangeCameraType(CameraType.TPS_NotAim);
        }

        
    }
}
