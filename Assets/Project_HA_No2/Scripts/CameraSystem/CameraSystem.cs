using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace HA
{
    public enum CameraType
    {
        TPS, QuaterView, FPS,
    }

    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; } = null;


        public void SetCameraFollowTarget(Transform target)
        {
            tpsCamera.Follow = target;
            tpsCamera.LookAt = target;
        }

        // 각각의 VirtualCamera GameObject
        public CinemachineVirtualCamera tpsCamera;
        public CinemachineVirtualCamera quaterCamera;
        public CinemachineVirtualCamera fpsCamera;

        public Vector3 AimingPoint { get; private set; }

        public LayerMask aimingLayerMask;

        private CameraType currentCameraType = CameraType.TPS;
        private bool isZoom = false;

        private void Awake()
        {
            Instance = this;
        }

        public void ChangeCameraType(CameraType newType)
        {
            // 파라미터로 넘어온 카메라 타입 값을 currentCameraType에 저장한다.
            currentCameraType = newType;

            // 일단 모든 카메라를 꺼준다.
            tpsCamera.gameObject.SetActive(false);
            quaterCamera.gameObject.SetActive(false);
            fpsCamera.gameObject.SetActive(false);

            // 새로운 카메라 타입에 해당하는 카메라를 켜준다.
            switch (currentCameraType)
            {
                case CameraType.TPS:
                    tpsCamera.gameObject.SetActive(true);
                    break;
                case CameraType.QuaterView:
                    quaterCamera.gameObject.SetActive(true);
                    break;
                case CameraType.FPS:
                    fpsCamera.gameObject.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            // F1,F2,F3키를 누를 때, 각각의 대응되는 Virtual Camera 의 GameObject를 켜준다.
            // 해당하지 않는 Virtucal Camera의 gameObject는 꺼준다.
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ChangeCameraType(CameraType.TPS);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                ChangeCameraType(CameraType.QuaterView);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                ChangeCameraType(CameraType.FPS);
            }

            // 마우스 우 클릭(Right Button)이 다운이 됐을 때, 딱 1번만 들어오는 이벤트
            if (Input.GetMouseButtonDown(1))
            {
                isZoom = !isZoom;
            }

            float targetFov = isZoom ? 20f : 60f;
            tpsCamera.m_Lens.FieldOfView = Mathf.Lerp(tpsCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * 5);

            // Aiming Point 계산
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, aimingLayerMask,
                QueryTriggerInteraction.Ignore))
            {
                AimingPoint = hitInfo.point;
            }
            else
            {
                AimingPoint = ray.GetPoint(100f);
            }
        }
    }
}
