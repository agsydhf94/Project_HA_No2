using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemEffectSO : ScriptableObject
    {
        [TextArea]
        public string effectDescription;

        public virtual void ExecuteEffect(Transform targetTransform)
        {
            Debug.Log("����Ʈ �����");
        }
    }
}
