using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace HA
{
    public class SaveManager : SingletonBase<SaveManager>
    {
        [SerializeField] private string fileName;

        private readonly HashSet<string> scenesToLoad 
            = new HashSet<string> { "TestScene" };

        private GameData gameData;
        private List<ISaveManager> saveManagers;
        private FileDataHandler dataHandler;

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            saveManagers = FindAllSaveManagers();
        }

        #region Scene Information Refresh

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"¾À ·ÎµåµÊ: {scene.name}");

            if (scenesToLoad.Contains(scene.name))
            {
                RefreshSaveManagers();
                LoadGame();
            }
        }

        #endregion

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

        
        public void RefreshSaveManagers()
        {
            saveManagers = FindAllSaveManagers();
        }

        [ContextMenu("Delete Save File")]
        public void DeleteSaveData()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            dataHandler.DeleteData();
        }

        public bool HasSavedData()
        {
            if (dataHandler.Load() != null)
            {
                return true;
            }

            return false;
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}
