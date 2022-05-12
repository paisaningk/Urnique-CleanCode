using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Script.Save
{
    public static class SaveSysetm 
    {
        public static void SavePlayer(SaveData saveData)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/Player.fun";
            FileStream stream = new FileStream(path, FileMode.Create);

            // SaveData data = new SaveData(saveData);
        
            // formatter.Serialize(stream,data);
            stream.Close();
        }

        public static SaveData LoadPlayer()
        {
            string path = Application.persistentDataPath + "/Player.fun";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
            
                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in" + path);
                return null;
            }
        
        }
    
    }
}
