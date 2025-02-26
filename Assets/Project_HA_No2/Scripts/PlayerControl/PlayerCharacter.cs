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

        public void CharacterMove(Vector2 input)
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

            characterAnimator.SetFloat("Horizontal", horizontal);
            characterAnimator.SetFloat("Vertical", vertical);
            characterAnimator.SetFloat("RunningBlend", runningBlend);

            characterController.Move(movement * Time.deltaTime * movingSpeed);
        }


    }
}