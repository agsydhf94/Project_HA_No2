using HA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Hit VFX", menuName = "DataSO/ItemEffect/Fire and Ice VFX")]
public class FireAndIce_EffectSO : ItemEffectSO
{
    [SerializeField] private GameObject iceVFX;
    [SerializeField] private GameObject fireVFX;

    public override void ExecuteEffect(Transform targetTransform)
    {
        GameObject iceEffect = Instantiate(iceVFX, targetTransform.position, Quaternion.identity);
        iceEffect.transform.position += new Vector3(0f, 2f, 0f);
        Destroy(iceEffect, 1.5f);

        GameObject fireEffect = Instantiate(fireVFX, targetTransform.position, Quaternion.identity);
        fireEffect.transform.position += new Vector3(0f, 2f, 0f);
        Destroy(fireEffect, 1f);
    }
}
