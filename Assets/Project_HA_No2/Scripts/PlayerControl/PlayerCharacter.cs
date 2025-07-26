using UnityEngine;
using UnityEngine.Animations.Rigging;

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
        public PlayerReloadState reloadState { get; private set; }
        public PlayerRifleArmedState rifleArmedState { get; private set; }
        public PlayerPrimaryAttackState primaryAttackState { get; private set; }
        public PlayerCounterAttackState counterAttackState { get; private set; }
        public PlayerAimBallState aimBallState { get; private set; }
        public PlayerThrowState throwBallState { get; private set; }
        public PlayerBlackHoleState blackHoleState { get; private set; }
        public PlayerDeadState deadState { get; private set; }
        #endregion


        #region Player Character Camera
        [Header("Player Character Camera")]       
        public CameraSystem cameraSystem;
        public Camera mainCamera;
        public Transform cameraPivot;
        public float bottomClamp = -90f;
        public float topClamp = 90f;
        private float targetYaw;
        private float targetPitch;
        #endregion

        #region Player Character Controller
        [Header("Player Character Controller")]
        public CharacterController characterController;
        #endregion

        #region Player Character Skill Components
        public SkillManager skillManager;
        public DetectTargetOnScreen detectTargetOnScreen;
        #endregion

        #region Player Moving Values
        [Header("Player Moving Values")]
        public Vector3 playerMovementVec;
        public float currentWalkingSpeedDelta;
        public float currentRunningSpeedDelta;
        public float subWalkingSpeedDelta;
        public float subRunningSpeedDelta;

        #endregion

        #region Player Shooting Values
        [SerializeField] private LayerMask aimLayerMask;
        public bool isAiming;
        public GameObject debugObject;
        #endregion

        #region Player Rigging
        public RigBuilder rigBuilder;
        #endregion

        #region Multipliers
        [Header("Multipliers")]
        public float moveSpeedMultiplier;
        public float jumpForceMultiplier;
        public float dashSpeedMultiplier;
        private float defaultMoveSpeedMultiplier;
        private float defaultJumpForceMultiplier;
        private float defaultDashSpeedMultiplier;
        #endregion

        #region Player Attack
        public float counterAttackDuration;
        public Vector3 mouseWorldPosition;
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
        [Header("Player Dash")]
        public float dashSpeed;
        public float dashDuration;
        #endregion

        #region Player Cosmetics
        [Header("Player Cosmetics")]
        public TrailRenderer trailRenderer;
        #endregion

        private Inventory inventory;
        public InputSystem inputSystem;
        public WeaponHandler weaponHandler;
        public ComboAttackHandler comboAttackHandler;
        public CanvasUI canvasUI;


        protected override void Awake()
        {
            base.Awake();
            rigBuilder = GetComponent<RigBuilder>();
            trailRenderer = GetComponent<TrailRenderer>();
            inputSystem = InputSystem.Instance;
            weaponHandler = GetComponent<WeaponHandler>();
            comboAttackHandler = GetComponent<ComboAttackHandler>();
            cameraSystem = CameraSystem.Instance;
            canvasUI = CanvasUI.Instance;
            mainCamera = Camera.main;
            stateMachine = new PlayerStateMachine();
            skillManager = SkillManager.Instance;
            inventory = Inventory.Instance;
            detectTargetOnScreen = DetectTargetOnScreen.Instance;
            characterController = GetComponent<CharacterController>();

            idleState = new PlayerIdleState(this, stateMachine, "Idle");
            moveState = new PlayerMoveState(this, stateMachine, "Move");
            jumpState = new PlayerJumpState(this, stateMachine, "Jump");
            airState = new PlayerAirState(this, stateMachine, "Air");
            dashState = new PlayerDashState(this, stateMachine, "Dash");
            armedState = new PlayerArmedState(this, stateMachine, "Armed");
            reloadState = new PlayerReloadState(this, stateMachine, "Reload");
            rifleArmedState = new PlayerRifleArmedState(this, stateMachine, "RifleArmed");
            primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
            counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
            aimBallState = new PlayerAimBallState(this, stateMachine, "AimBall");
            throwBallState = new PlayerThrowState(this, stateMachine, "ThrowBall");
            blackHoleState = new PlayerBlackHoleState(this, stateMachine, "BlackHole");
            deadState = new PlayerDeadState(this, stateMachine, "Dead");
        }

        protected override void Start()
        {
            stateMachine.Initialize(idleState);

            defaultMoveSpeedMultiplier = moveSpeedMultiplier;
            defaultJumpForceMultiplier = jumpForceMultiplier;
            defaultDashSpeedMultiplier = dashSpeedMultiplier;
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.currentState.UpdateState();
            
            if(stateMachine.currentSubState != null)
                stateMachine.currentSubState.UpdateState();

            CheckDashInput();

            if (Input.GetKeyDown(KeyCode.Alpha8) && inventory.GetEquipment(EquipmentType.Potion) != null)
            {
                inventory.UsePotion();
            }
        }

        #region Player Slow Effect

        public override void GetSlowBy(float percentage, float slowDuration)
        {
            moveSpeedMultiplier *= percentage;
            jumpForceMultiplier *= percentage;
            dashSpeedMultiplier *= percentage;
            characterAnimator.speed *= percentage;

            Invoke("ReturnDefaultSpeed", slowDuration);
        }
        

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();

            moveSpeedMultiplier = defaultMoveSpeedMultiplier;
            jumpForceMultiplier = defaultJumpForceMultiplier;
            dashSpeedMultiplier = defaultDashSpeedMultiplier;
        }
        #endregion

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

                if (stateMachine.currentSubState != null)
                    movingSpeed += subRunningSpeedDelta;
            }
            else
            {
                movingSpeed += currentWalkingSpeedDelta;

                if (stateMachine.currentSubState != null)
                    movingSpeed += subWalkingSpeedDelta;
            }
            runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);


            // ���콺�� ī�޶� ȸ�� �� �̵� �� �̵� ���� ���� ����
            // Mathf.Atan2 ���� Horizontal ������ �����Ͽ� ���� ���⿡ ���� ���Ÿ� ���
            Vector3 movement = transform.forward * vertical + transform.right * horizontal;
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

            playerMovementVec = movement * Time.deltaTime * movingSpeed * moveSpeedMultiplier;
            characterController.Move(playerMovementVec);
        }

        public void Character_SetZeroVelocity()
        {
            // �̵� �ӵ��� ����ϰ� ������ �� �̲������� �ȴ�.
            playerMovementVec = Vector3.zero;  // �̵� ���� �ʱ�ȭ
            characterController.Move(Vector3.zero);
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
            Vector3 jumpMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0) * jumpForceMultiplier;
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
            // ���� ĳ������ Forward ������ �������� XZ ��� ���� ����
            Vector3 currentCharacterForward_xz = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

            // �÷��̾� �̵� ������ �������� XZ ��� ���� ����
            Vector3 planeDirection_xz = new Vector3(playerMovementVec.x, 0, playerMovementVec.z).normalized;

            // ���� ����� ��ǥ ������ ���� ���� ��� (SignedAngle�� �ð�/�ݽð� ���� �Ǻ�)
            float angleDifference = Vector3.SignedAngle(currentCharacterForward_xz, planeDirection_xz, Vector3.up);

            // Y�� ȸ�� ���� (�ε巴�� ȸ��)
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angleDifference, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f); // �ӵ� ���� ����

            // XZ ���󿡼� �̵�
            characterController.Move(playerMovementVec + planeDirection_xz * dashSpeed * dashSpeedMultiplier);
        }

        public void CheckDashInput()
        {
            if (skillManager.dashSkill.dashUnlocked == false)
                return;

            if(Input.GetKeyDown(KeyCode.C) && skillManager.dashSkill.CanUseSkill())
            {
                stateMachine.ChangeState(dashState);
            }
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

        public void CharacterRifleArmed()
        {
            stateMachine.SubState_On(rifleArmedState);
        }

        public void CharacterRifleUnArmed()
        {
            stateMachine.SubState_Off();
        }
        #endregion

        #region Character Life Related
        public override void Die()
        {
            base.Die();

            stateMachine.ChangeState(deadState);
        }
        #endregion

        #region Check Stunnable Enemies
        public GameObject GetStunnableEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(attackCheck.position, attackCheckRadius);

            foreach(var hit in colliders)
            {
                if(hit.TryGetComponent<Enemy>(out Enemy enemy) && enemy.CanBeStunned())
                {
                    return hit.gameObject;
                }
            }
            return null;
        }
        #endregion

        #region Animation Control
        public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
        public void AnimationFinishTrigger_Sub() => stateMachine.currentSubState.AnimationFinishTrigger();
        #endregion

        #region Character State Control
        public void ExitBlackHoleSkill()
        {
            stateMachine.ChangeState(idleState);
        }
        #endregion

        #region Character Shooting Logic
        public void SetAimInput()
        {
            inputSystem.OnClickRightMouseButtonDown += cameraSystem.ZoomInToAim;
            inputSystem.OnClickRightMouseButtonUp += cameraSystem.ZoomOutToDefault;
        }

        public void RemoveAimInput()
        {
            inputSystem.OnClickRightMouseButtonDown -= cameraSystem.ZoomInToAim;
            inputSystem.OnClickRightMouseButtonUp -= cameraSystem.ZoomOutToDefault;
        }

        public void SetShootingPosition()
        {
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

            Ray ray = mainCamera.ScreenPointToRay(screenCenterPoint);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimLayerMask))
            {
                debugObject.transform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }

            if(isAiming)
            {
                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 22f);
            }

            this.mouseWorldPosition = mouseWorldPosition;
        }

        #endregion

        #region Trail Renderer System
        public void DashTrailRenderer_On() => trailRenderer.emitting = true;
        public void DashTrailRenderer_Off() => trailRenderer.emitting = false;
        #endregion

    }
}