using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
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
        public PlayerJumpState jumpState { get; private set; }
        public PlayerAirState airState { get; private set; }
        public PlayerDashState dashState { get; private set; }
        public PlayerArmedState armedState { get; private set; }
        #endregion


        #region Player Character Camera
        [Header("Player Character Camera")]
        public InputSystem inputSystem;
        public Camera mainCamera;
        public Transform cameraPivot;
        public float bottomClamp = -90f;
        public float topClamp = 90f;
        private float targetYaw;
        private float targetPitch;
        #endregion

        #region Collision Information
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private LayerMask groundLayer;
        #endregion

        #region Player Moving Values
        public Vector3 playerMovementVec;
        public float currentWalkingSpeedDelta;
        public float currentRunningSpeedDelta;
        public float subWalkingSpeedDelta;
        public float subRunningSpeedDelta;
        #endregion

        #region Player Jump
        [Header("Player Jump")]
        [SerializeField] private float maxJumpHeight;
        [SerializeField] private float maxJumpTime;
        private float modifiedGravity;
        public float verticalVelocity;
        public float stopDetectGroundDuration;
        public bool isFromJump;
        #endregion

        #region Player Dash
        public float dashSpeed;
        public float dashDuration;
        #endregion

        #region Player Cosmetics
        public TrailRenderer trailRenderer;
        #endregion


        public override void Awake()
        {
            base.Awake();
            trailRenderer = GetComponent<TrailRenderer>();
            inputSystem = InputSystem.Instance;
            mainCamera = Camera.main;
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, "Idle");
            moveState = new PlayerMoveState(this, stateMachine, "Move");
            jumpState = new PlayerJumpState(this, stateMachine, "Jump");
            airState = new PlayerAirState(this, stateMachine, "Air");
            dashState = new PlayerDashState(this, stateMachine, "Dash");
            armedState = new PlayerArmedState(this, stateMachine, "Armed");
        }

        private void Start()
        {
            stateMachine.Initialize(idleState);
        }

        public override void Update()
        {
            base.Update();

            stateMachine.currentState.UpdateState();
            
            if(stateMachine.subState != null)
                stateMachine.subState.UpdateState();
        }

        #region Character Move
        public void CharacterMove(Vector2 input, float yAxisAngle)
        {
            horizontal = input.x;
            vertical = input.y;


            float movingSpeed = basicSpeed;
            IsRun = inputSystem.IsRunKey;
            if(inputSystem.IsRunKey)
            {
                movingSpeed += currentRunningSpeedDelta;

                if (stateMachine.subState != null)
                    movingSpeed += subRunningSpeedDelta;
            }
            else
            {
                movingSpeed += currentWalkingSpeedDelta;

                if (stateMachine.subState != null)
                    movingSpeed += subWalkingSpeedDelta;
            }
            runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);


            // 마우스로 카메라 회전 후 이동 시 이동 방향 갱신 가능
            // Mathf.Atan2 에서 Horizontal 성분을 제거하여 전후 방향에 대한 갱신만 허용
            Vector3 movement = transform.forward * vertical + transform.right * horizontal;
            if (input.magnitude > 0f)
            {
                targetRotation = Mathf.Atan2(0, input.y) * Mathf.Rad2Deg + yAxisAngle;
                if (Mathf.Sign(vertical) >= 0)
                {    
                    // 앞으로 갈때는 targetRotation(정면)을 바라보도록 설정
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

            playerMovementVec = movement * Time.deltaTime * movingSpeed;
            characterController.Move(playerMovementVec);
        }
        #endregion

        #region Camera Rotation
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
        #endregion

        #region Gravity and Jump
        public void ApplyModifiedGravity() => verticalVelocity += modifiedGravity * Time.deltaTime;

        public void ApplyNaturalGravity()
        {
            Vector3 freefallVec = new Vector3(0, -9.81f * Time.deltaTime * 0.5f, 0);
            characterController.Move(playerMovementVec + freefallVec);
        }

        public void CharacterJump()
        {
            Vector3 jumpMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);
            characterController.Move(playerMovementVec + jumpMove);
        }

        public void GravityCalculate()
        {
            float timeToApex = maxJumpTime / 2;
            modifiedGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            verticalVelocity = (2 * maxJumpHeight) / timeToApex;        
        }
        #endregion

        #region Character Dash
        public void CharacterDash()
        {
            // 현재 캐릭터의 Forward 방향을 기준으로 XZ 평면 벡터 생성
            Vector3 currentCharacterForward_xz = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

            // 플레이어 이동 방향을 기준으로 XZ 평면 벡터 생성
            Vector3 planeDirection_xz = new Vector3(playerMovementVec.x, 0, playerMovementVec.z).normalized;

            // 현재 방향과 목표 방향의 각도 차이 계산 (SignedAngle로 시계/반시계 방향 판별)
            float angleDifference = Vector3.SignedAngle(currentCharacterForward_xz, planeDirection_xz, Vector3.up);

            // Y축 회전 적용 (부드럽게 회전)
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angleDifference, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f); // 속도 조절 가능

            // XZ 평면상에서 이동
            characterController.Move(playerMovementVec + planeDirection_xz * dashSpeed);
        }
        #endregion

        #region Character Armed Check
        public void CharacterArmed()
        {
            stateMachine.SubState_On(armedState);
            characterAnimator.SetTrigger("ArmedTrigger");
        }

        public void CharacterUnArmed()
        {
            stateMachine.SubState_Off();
            characterAnimator.SetTrigger("UnArmedTrigger");
        }
        #endregion

        #region Collision
        public bool IsGroundedDetected() => Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance); 
        }
        #endregion

        #region Trail Renderer System
        public void DashTrailRenderer_On() => trailRenderer.emitting = true;
        public void DashTrailRenderer_Off() => trailRenderer.emitting = false;
        #endregion


    }
}