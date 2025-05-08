using UnityEditor;
using UnityEngine;

namespace HA
{
    public class CheckPoint : MonoBehaviour
    {
        public CheckpointDataSO checkPointDataSO;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") 
                && CheckPointProgressManager.Instance.passedCheckpointIDs.Contains(checkPointDataSO.checkpointID)) return;

            CheckPointProgressManager.Instance.UpdateCheckpoint(checkPointDataSO.checkpointID);
        }
    }
}
