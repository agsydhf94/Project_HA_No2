using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HA
{
    public class SaveManager : SingletonBase<SaveManager>
    {
        [SerializeField] private string fileName;

        private GameData gameData;
        private List<ISaveManager> saveManagers;
        private FileDataHandler dataHandler;

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            saveManagers = FindAllSaveManagers();

            LoadGame();
        }

        public void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            gameData = dataHandler.Load();

            if(this.gameData == null)
            {
                Debug.Log("No Data Found");
                NewGame();
            }

            foreach(var saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            foreach(var saveManager in saveManagers)
            {
                saveManager.SaveData(ref gameData);
            }

            dataHandler.Save(gameData);
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
