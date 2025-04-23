using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "Freeze Enemy effect", menuName = "DataSO/ItemEffect/Freeze Enemy")]
    public class FreezeEnemyEffectSO : ItemEffectSO
    {
        [SerializeField] private float duration;

        public override void ExecuteEffect(Transform transform)
        {
            PlayerStat playerStat = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();
            if (playerStat.currentHp > playerStat.GetMaxHealthValue() * 0.1f)
                return;

            if(!Inventory.Instance.CanUseArmor())
                return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

            foreach(var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            }
        }
    }
}
