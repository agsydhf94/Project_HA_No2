using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class AiContext : MonoBehaviour
    {
        public Dictionary<string, IAiAction> aiActionStates = new Dictionary<string, IAiAction>();
        public AiStateRunner runner; // 자기 FSM을 참조해도 좋음

        public void AddState(string key, IAiAction aiAction)
        {
            aiActionStates.Add(key, aiAction);
        }

        public IAiAction GetState(string key)
        {
            return aiActionStates[key];
        }
    }
}
