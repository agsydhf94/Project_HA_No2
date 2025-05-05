using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace HA
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string _dataDirectionPath, string _dataFileName)
        {
            dataDirPath = _dataDirectionPath;
            dataFileName = _dataFileName;
        }

        public void Save(GameData _data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(_data, true);

                using(FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError("Save Error" + fullPath + "\n" + e);
            }
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadData = null;

            if(File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch(Exception e)
                {
                    Debug.LogError("Load Error" + fullPath + "\n" + e);
                }
            }

            return loadData;
        }
    }
}
