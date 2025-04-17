using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "Sword Hit VFX", menuName = "DataSO/ItemEffect/Thunder Strike VFX")]
    public class ThunderStrike_EffectSO : ItemEffectSO
    {
        [SerializeField] private GameObject swordHitVFX;

        public override void ExecuteEffect(Transform targetTransform)
        {
            GameObject thunderStrikeVFX = Instantiate(swordHitVFX, targetTransform.position, Quaternion.identity);
            thunderStrikeVFX.transform.position += new Vector3(0f, 2f, 0f);
            Destroy(thunderStrikeVFX, 1f);
        }
    }
}
