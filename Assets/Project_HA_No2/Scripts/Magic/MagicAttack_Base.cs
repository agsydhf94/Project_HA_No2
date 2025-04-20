using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class MagicAttack_Base : MonoBehaviour
    {
        protected PlayerStat playerStat;
        protected PlayerManager playerManager;

        protected virtual void Start()
        {
            playerManager = PlayerManager.Instance;
            playerStat = playerManager.playerCharacter.GetComponent<PlayerStat>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Enemy>() != null)
            {
                EnemyStat enemyStat = other.GetComponent<EnemyStat>();
                playerStat.DoMagicalDamage(enemyStat);
            }
        }
    }
}
