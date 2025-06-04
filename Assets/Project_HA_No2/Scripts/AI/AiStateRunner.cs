using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class AiStateRunner : MonoBehaviour
    {
        public IAiAction currentState;
        public IAiAction subState;

        public void SetCurrentState(IAiAction state) => currentState = state;
        public void SetSubState(IAiAction state) => subState = state;


        private void Update()
        {
            RunUpdate();
        }


        public void RunEnter()
        {
            currentState?.OnEnter();
            subState?.OnEnter();
        }

        public void RunUpdate()
        {
            currentState?.OnUpdate();
            subState?.OnUpdate();
        }

        public void RunExit()
        {
            currentState?.OnExit();
            subState?.OnExit();
        }

        public void Initialize(IAiAction aiAction)
        {
            currentState = aiAction;
            RunEnter();
        }

        public void ChangeState(IAiAction newAiAction)
        {
            RunExit();
            currentState = newAiAction;
            RunEnter();
        }
    }
}
