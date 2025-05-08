using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CheckPointProgressManager : SingletonBase<CheckPointProgressManager>, ISaveManager
    {
        public HashSet<string> passedCheckpointIDs = new();
        private string lastCheckpointID;

        private string textInput = "CheckPoint Reached";

        [SerializeField] private Color blinkingColor;

        public void UpdateCheckpoint(string checkpointID)
        {
            if (!passedCheckpointIDs.Contains(checkpointID))
            {
                passedCheckpointIDs.Add(checkpointID);
                lastCheckpointID = checkpointID;

                Debug.Log($"[Checkpoint] Latest checkpoint: {checkpointID}");
                CheckPopUpUI.Instance.StartUIAnimationSequence(textInput, blinkingColor);
            }
        }

        public bool HasPassed(string checkpointID)
        {
            return passedCheckpointIDs.Contains(checkpointID);
        }

        public string GetLastCheckpointID()
        {
            return lastCheckpointID;
        }

        public void LoadData(GameData _data)
        {
            //passedCheckpointIDs = new HashSet<string>(_data.passedCheckpointIDs);

            foreach(string passedCheckpointID in _data.passedCheckpointIDs)
            {
                passedCheckpointIDs.Add(passedCheckpointID);
            }

            lastCheckpointID = _data.lastCheckpointID;
        }

        public void SaveData(ref GameData _data)
        {
            _data.passedCheckpointIDs.Clear();
            _data.lastCheckpointID = null;


            foreach(var passedCheckpointID in passedCheckpointIDs)
            {
                _data.passedCheckpointIDs.Add(passedCheckpointID);
            }

            _data.lastCheckpointID = lastCheckpointID;

            Debug.Log("Checkpoint Data Saved");
        }
    }
}
