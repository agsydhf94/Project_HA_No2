using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HA
{
    public class SaveManager : SingletonBase<SaveManager>
    {
        private GameData gameData;
        private List<ISaveManager> saveManagers;

        private void Start()
        {
            saveManagers = FindAllSaveManagers();

            LoadGame();
        }

        public void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            if(this.gameData == null)
            {
                Debug.Log("No Game Data Found");
                NewGame();
            }

            foreach(var saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
                Debug.Log("Game Data Loaded");
            }
        }

        public void SaveGame()
        {
            foreach(var saveManager in saveManagers)
            {
                saveManager.SaveData(ref gameData);
                Debug.Log("Game Data Saved");
            }
        }

        private List<ISaveManager> FindAllSaveManagers()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}
