using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStateMachine stateMachine { get; private set; }

        public PlayerIdleState idleState { get; private set; }
        public PlayerMoveState moveState { get; private set; }


        private void Awake()
        {
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, "Idle");
            moveState = new PlayerMoveState(this, stateMachine, "Move");
            
        }


    }
}