using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerManager : SingletonBase<PlayerManager>, ISaveManager
    {
        public PlayerCharacter playerCharacter;
        public int currency;

        public bool CheckEnoughMoney(int price)
        {
            if(price > currency)
            {
                Debug.Log("돈이 부족합니다");
                return false;
            }

            currency -= price;
            return true;
        }

        public int GetCurrentMoney() => currency;


        public void LoadData(GameData _data)
        {
            this.currency = _data.currency;
        }

        public void SaveData(ref GameData _data)
        {
            _data.currency = this.currency;
        }
    }
}
