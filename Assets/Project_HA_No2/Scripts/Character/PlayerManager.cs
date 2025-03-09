using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerManager : SingletonBase<PlayerManager>
    {
        public PlayerCharacter playerCharacter;

        public override void Awake()
        {
            playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        }
    }
}
