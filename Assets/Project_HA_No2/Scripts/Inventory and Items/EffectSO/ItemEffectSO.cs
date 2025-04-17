using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemEffectSO : ScriptableObject
    {
        public virtual void ExecuteEffect(Transform targetTransform)
        {
            Debug.Log("ÀÌÆåÆ® Àç»ýµÊ");
        }
    }
}
