using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "CheckpointData", menuName = "Game/Checkpoint Data")]
    public class CheckpointDataSO : ScriptableObject
    {
        public string checkpointID;
        public string sceneName;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        public bool triggerCutscene;
    }
}
