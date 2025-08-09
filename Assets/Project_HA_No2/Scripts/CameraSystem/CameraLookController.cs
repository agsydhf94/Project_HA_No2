using UnityEngine;

namespace HA
{
    /// <summary>
    /// Handles camera rotation based on player look input.
    /// 
    /// Features:
    /// - Reads yaw (horizontal) and pitch (vertical) input from <see cref="InputSystem"/>.
    /// - Applies different sensitivities depending on aiming state.
    /// - Clamps vertical rotation to prevent over-rotation.
    /// - Updates the <see cref="cameraPivot"/> transform's rotation accordingly.
    /// 
    /// Usage:
    /// Attach this to a camera controller GameObject.
    /// Assign <see cref="playerBody"/> to the root player transform, and <see cref="cameraPivot"/> 
    /// to the transform that serves as the rotation pivot for the camera.
    /// </summary>
    public class CameraLookController : MonoBehaviour
    {
        /// <summary>
        /// The root transform of the player's body, used for yaw rotation reference.
        /// </summary>
        public Transform playerBody;

        /// <summary>
        /// The pivot point of the camera, which is rotated based on input.
        /// </summary>
        public Transform cameraPivot;

        private CameraSystem cameraSystem;
        private InputSystem inputSystem;

        [Header("Camera Moving")]
        /// <summary>
        /// Sensitivity when not aiming (degrees per second).
        /// </summary>
        public float baseSensitivity = 150f;

        /// <summary>
        /// Sensitivity when aiming (degrees per second).
        /// </summary>
        public float aimSensitivity = 75f;

        /// <summary>
        /// Minimum pitch angle allowed (looking down).
        /// </summary>
        public float bottomClamp = -90f;

        /// <summary>
        /// Maximum pitch angle allowed (looking up).
        /// </summary>
        public float topClamp = 90f;

        /// <summary>
        /// Current yaw rotation in degrees.
        /// </summary>
        private float targetYaw;

        /// <summary>
        /// Current pitch rotation in degrees.
        /// </summary>
        private float targetPitch;

        /// <summary>
        /// Reserved for potential smoothing or extra rotation control.
        /// </summary>
        private float xRotation = 0f;

        void Awake()
        {
            cameraSystem = CameraSystem.Instance;
            inputSystem = InputSystem.Instance;
        }

        void Update()
        {
            CameraRotation();
        }


        /// <summary>
        /// Updates yaw and pitch based on player input, clamps pitch, and applies rotation to the camera pivot.
        /// </summary>
        public void CameraRotation()
        {
            if (inputSystem.Look.sqrMagnitude > 0f)
            {
                float sensitivity = CameraSystem.Instance.InstanceIsAiming() ? aimSensitivity : baseSensitivity;
                float yaw = inputSystem.Look.x * sensitivity * Time.deltaTime;
                float pitch = inputSystem.Look.y * sensitivity * Time.deltaTime;

                targetYaw += yaw;
                targetPitch -= pitch;
            }

            targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
            targetPitch = ClampAngle(targetPitch, bottomClamp, topClamp);
            cameraPivot.rotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }


        /// <summary>
        /// Clamps an angle to a specified range, wrapping around if necessary.
        /// </summary>
        /// <param name="lfAngle">Angle to clamp, in degrees.</param>
        /// <param name="lfMin">Minimum allowed value.</param>
        /// <param name="lfMax">Maximum allowed value.</param>
        /// <returns>Clamped angle in degrees.</returns>
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
