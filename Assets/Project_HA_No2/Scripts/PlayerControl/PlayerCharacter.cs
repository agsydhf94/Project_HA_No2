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
        public PlayerJumpState jumpState { get; private set; }
        public PlayerAirState airState { get; private set; }
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
        #endregion

        #region Player Jump
        [Header("Player Jump")]
        [SerializeField] private float gravity;
        [SerializeField] private float maxJumpHeight;
        [SerializeField] private float maxJumpTime;
        public float verticalVelocity;
        public bool isGroundDetected;
        #endregion


        public override void Awake()
        {
            base.Awake();
            inputSystem = InputSystem.Instance;
            mainCamera = Camera.main;
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, "Idle");
            moveState = new PlayerMoveState(this, stateMachine, "Move");
            jumpState = new PlayerJumpState(this, stateMachine, "Jump");
            airState = new PlayerAirState(this, stateMachine, "Jump");
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

        #region Character Move
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

            // ���콺�� ī�޶� ȸ�� �� �̵� �� �̵� ���� ���� ����
            // Mathf.Atan2 ���� Horizontal ������ �����Ͽ� ���� ���⿡ ���� ���Ÿ� ���
            if (input.magnitude > 0f)
            {
                targetRotation = Mathf.Atan2(0, input.y) * Mathf.Rad2Deg + yAxisAngle;
                if (Mathf.Sign(vertical) >= 0)
                {    
                    // ������ ������ targetRotation(����)�� �ٶ󺸵��� ����
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationSpeed, 0.1f);
                    transform.rotation = Quaternion.Euler(0f, rotation, 0f);
                }
                else
                {
                    // �ڷ� ������ targetRotation�� 180�� �߰������μ� �ް����ϴ� ����� ����
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
        public void ApplyGravity() => verticalVelocity += gravity * Time.deltaTime;

        public void CharacterJump()
        {
            Vector3 jumpMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);
            characterController.Move(playerMovementVec + jumpMove);
        }

        public void GravityCalculate()
        {
            float timeToApex = maxJumpTime / 2;
            gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            verticalVelocity = (2 * maxJumpHeight) / timeToApex;        
        }
        #endregion


        #region Collision
        public bool IsGroundedDetected() => Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance); 
        }
        #endregion


    }
}