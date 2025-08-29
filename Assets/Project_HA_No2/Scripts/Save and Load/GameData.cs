using System.Collections.Generic;

namespace HA
{
    [System.Serializable]
    public class GameData
    {
        /// <summary>
        /// Player character's current level.
        /// </summary>
        public int currentCharacterLevel;

        /// <summary>
        /// Player character's current XP toward the next level.
        /// </summary>
        public int currentXp;

        /// <summary>
        /// Player's current money/currency.
        /// </summary>
        public int currency;

        /// <summary>
        /// List of checkpoint IDs the player has passed.
        /// </summary>
        public List<string> passedCheckpointIDs;

        /// <summary>
        /// ID of the last checkpoint the player reached.
        /// </summary>
        public string lastCheckpointID;

        /// <summary>
        /// Name of the last scene the player was in.
        /// </summary>
        public string lastSceneName;

        /// <summary>
        /// Player’s unlocked skills stored in a key–value dictionary.
        /// </summary>
        public SerializableDictionary<string, bool> skillTree;

        /// <summary>
        /// Player’s inventory stored as item ID–quantity dictionary.
        /// </summary>
        public SerializableDictionary<string, int> inventory;

        /// <summary>
        /// List of currently equipped item IDs.
        /// </summary>
        public List<string> equipmentId;


        /// <summary>
        /// Default constructor initializing empty collections and initial values.
        /// </summary>
        public GameData()
        {
            currentCharacterLevel = 1;
            currentXp = 0;
            currency = 0;

            passedCheckpointIDs = new List<string>();
            lastCheckpointID = null;
            lastSceneName = "TestScene";

            skillTree = new SerializableDictionary<string, bool>();
            inventory = new SerializableDictionary<string, int>();
            equipmentId = new List<string>();
        }
    }
}
