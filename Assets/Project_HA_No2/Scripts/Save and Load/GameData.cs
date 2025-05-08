using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [System.Serializable]
    public class GameData
    {
        public int currency;

        public List<string> passedCheckpointIDs;
        public string lastCheckpointID;

        public SerializableDictionary<string, bool> skillTree;
        public SerializableDictionary<string, int> inventory;
        public List<string> equipmentId;

        public GameData()
        {
            this.currency = 0;

            passedCheckpointIDs = new List<string>();
            lastCheckpointID = null;

            skillTree = new SerializableDictionary<string, bool>();
            inventory = new SerializableDictionary<string, int>();
            equipmentId = new List<string>();
        }
    }
}
