using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace HA
{

    [CreateAssetMenu(fileName = "Heal effect", menuName = "DataSO/ItemEffect/Heal VFX")]
    public class HealEffectSO : ItemEffectSO
    {
        [Range(0f, 1f)]
        [SerializeField] private float healPercent;

        public override void ExecuteEffect(Transform targetTransform)
        {
            PlayerStat playerStat = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();

            int healAmount = Mathf.RoundToInt(playerStat.GetMaxHealthValue() * healPercent);
            playerStat.IncreaseHealthBy(healAmount);   
        }
    }
}
