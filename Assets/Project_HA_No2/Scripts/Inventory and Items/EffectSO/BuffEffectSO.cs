using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "Buff effect", menuName = "DataSO/ItemEffect/Buff")]
    public class BuffEffectSO : ItemEffectSO
    {
        private PlayerStat stats;
        [SerializeField] private StatType buffType;
        [SerializeField] private int buffAmount;
        [SerializeField] private int buffDuration;

        public override void ExecuteEffect(Transform enemyPosition)
        {
            stats = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();
            stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
        }

        
    }
}
