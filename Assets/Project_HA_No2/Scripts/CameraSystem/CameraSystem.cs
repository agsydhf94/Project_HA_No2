using UnityEngine;
using Cinemachine;

namespace HA
{
    /// <summary>
    /// Manages camera behavior and transitions between different gameplay camera types.
    /// Integrates with Cinemachine for smooth camera switching, aiming zoom, and camera shake effects.
    /// </summary>
    public enum CameraType
    {
        /// <summary>
        /// Third-person camera without aiming mode.
        /// </summary>
        TPS_NotAim,

        /// <summary>
        /// Third-person camera with aiming mode enabled.
        /// </summary>
        TPS_Aim,

        /// <summary>
        /// Camera used for NPC cinematic or cutscenes.
        /// </summary>
        NPC
    }


    /// <summary>
    /// Centralized camera system that controls Cinemachine virtual cameras for TPS gameplay and NPC views.
    /// Supports camera shake, zoom transitions, and retrieving the active camera transform.
    /// </summary>
    public class CameraSystem : SingletonBase<CameraSystem>
    {
        /// <summary>
        /// Third-person virtual camera for non-aiming mode.
        /// </summary>
        public CinemachineVirtualCamera tpsCamera_NotAim;

        /// <summary>
        /// Third-person virtual camera for aiming mode.
        /// </summary>
        public CinemachineVirtualCamera tpsCamera_Aim;

        /// <summary>
        /// Virtual camera for NPC or cinematic sequences.
        /// </summary>
        public CinemachineVirtualCamera npcCamera;

        /// <summary>
        /// Impulse source used for generating camera shake effects.
        /// </summary>
        public CinemachineImpulseSource impulseSource;

        /// <summary>
        /// Current aiming point in world space.
        /// </summary>
        public Vector3 AimingPoint { get; private set; }

        /// <summary>
        /// Layer mask used for aiming raycasts.
        /// </summary>
        public LayerMask aimingLayerMask;

        private CameraType currentCameraType = CameraType.TPS_NotAim;

        /// <summary>
        /// Reference to the main Unity camera.
        /// </summary>
        public Camera mainCamera;

        private bool isZoom = false;

        /// <summary>
        /// Initializes the camera system and sets default camera priorities.
        /// </summary>
        public override void Awake()
        {
            tpsCamera_NotAim.Priority = 11;
            tpsCamera_Aim.Priority = 10;
            mainCamera = Camera.main;
        }


        /// <summary>
        /// Sets the follow and look-at targets for all relevant virtual cameras.
        /// </summary>
        /// <param name="target">The transform to follow and look at.</param>
        public void SetCameraFollowTarget(Transform target)
        {
            tpsCamera_NotAim.Follow = target;
            tpsCamera_NotAim.LookAt = target;

            tpsCamera_Aim.Follow = target;
            tpsCamera_Aim.LookAt = target;
        }


        /// <summary>
        /// Changes the active camera type by adjusting Cinemachine priorities.
        /// </summary>
        /// <param name="newType">The new camera type to activate.</param>
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


        /// <summary>
        /// Triggers a camera shake effect using the impulse source.
        /// </summary>
        /// <param name="velocity">The impulse velocity vector.</param>
        /// <param name="duration">How long the shake effect lasts.</param>
        /// <param name="force">The intensity of the shake.</param>
        public void ShakeCamera(Vector3 velocity, float duration, float force)
        {
            impulseSource.m_DefaultVelocity = velocity;
            impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
            impulseSource.GenerateImpulseWithForce(force);
        }


        /// <summary>
        /// Switches the camera to aiming mode (zoomed-in).
        /// </summary>
        public void ZoomInToAim()
        {
            ChangeCameraType(CameraType.TPS_Aim);
        }


        /// <summary>
        /// Switches the camera to default non-aiming mode (zoomed-out).
        /// </summary>
        public void ZoomOutToDefault()
        {
            ChangeCameraType(CameraType.TPS_NotAim);
        }


        /// <summary>
        /// Retrieves the transform of the currently active output camera.
        /// </summary>
        /// <returns>The transform of the active camera, or Camera.main as a fallback.</returns>
        public Transform GetCameraTransform()
        {
            var brain = Camera.main?.GetComponent<CinemachineBrain>();
            if (brain != null && brain.OutputCamera != null)
            {
                return brain.OutputCamera.transform;
            }

            Debug.LogWarning("CameraSystem: OutputCamera is null. Returning fallback Camera.main.transform");
            return Camera.main?.transform;
        }


        /// <summary>
        /// Checks whether the camera is currently in aiming mode.
        /// </summary>
        /// <returns>True if the active camera is TPS_Aim, false otherwise.</returns>
        public bool InstanceIsAiming()
        {
            return currentCameraType == CameraType.TPS_Aim;
        }

    }
}
