using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace HA
{
    public class PlayerCharacter : CharacterBase
    {

        #region PlayerStates
        public PlayerStateMachine stateMachine { get; private set; }
        public PlayerIdleState idleState { get; private set; }
        public PlayerMoveState moveState { get; private set; }
        #endregion


        #region Components
        public InputSystem inputSystem;
        public Camera mainCamera;
        public Transform cameraPivot;
        public float bottomClamp = -90f;
        public float topClamp = 90f;
        private float targetYaw;
        private float targetPitch;
        #endregion

        public override void Awake()
        {
            base.Awake();
            inputSystem = InputSystem.Instance;
            mainCamera = Camera.main;
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, "Idle");
            moveState = new PlayerMoveState(this, stateMachine, "Move");
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
        }

        public override void Update()
        {
            base.Update();

            stateMachine.currentState.UpdateState();
        }

        public void CharacterMove(Vector2 input, float yAxisAngle)
        {
            horizontal = input.x;
            vertical = input.y;
            unitSpeed = input.magnitude > 0f ? 1f : 0f;

            Vector3 movement = transform.forward * vertical + transform.right * horizontal;

            IsRun = inputSystem.IsRunKey;
            if(inputSystem.IsRunKey)
            {
                movingSpeed = 2f;
            }
            runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);

            // 마우스로 카메라 회전 후 이동 시 이동 방향 갱신 가능
            // Mathf.Atan2 에서 Horizontal 성분을 제거하여 전후 방향에 대한 갱신만 허용
            if (input.magnitude > 0f)
            {
                targetRotation = Mathf.Atan2(0, input.y) * Mathf.Rad2Deg + yAxisAngle;
                if (Mathf.Sign(vertical) >= 0)
                {    
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationSpeed, 0.1f);
                    transform.rotation = Quaternion.Euler(0f, rotation, 0f);
                }
                else
                {
                    // 뒤로 갈때는 targetRotation에 180을 추가함으로서 뒷걸음하는 모습을 연출
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation + 180f, ref rotationSpeed, 0.1f);
                    transform.rotation = Quaternion.Euler(0f, rotation, 0f);
                }
                
            }

            characterAnimator.SetFloat("Horizontal", horizontal);
            characterAnimator.SetFloat("Vertical", vertical);
            characterAnimator.SetFloat("RunningBlend", runningBlend);

            characterController.Move(movement * Time.deltaTime * movingSpeed);
        }

        public void CameraRotation()
        {
            if (inputSystem.Look.sqrMagnitude > 0f)
            {
                float yaw = inputSystem.Look.x;
                float pitch = inputSystem.Look.y;

                targetYaw += yaw;
                targetPitch -= pitch;
            }

            targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
            targetPitch = ClampAngle(targetPitch, bottomClamp, topClamp);
            cameraPivot.rotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }

        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

    }
}