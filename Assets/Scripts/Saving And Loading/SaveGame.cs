using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveGame
{
    public static void Save( CharacterPanel cp, Inventory inv, ItemManager im, ToDo td, BackgroundSelector bg, AudioManager am)
    {
        PlayerData data = new PlayerData(cp, inv, im, td, bg, am);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveData.txt", json);
        //Debug.Log(json);
    }

    //deserialize saved data
    public static PlayerData Load()
    {
        try
        {
            string path = Application.persistentDataPath + "/saveData.txt";

            //if there is a save file continue
            if (File.Exists(path))
            {
                string saveString = File.ReadAllText(Application.persistentDataPath + "/saveData.txt");
                PlayerData data = JsonUtility.FromJson<PlayerData>(saveString);
                return data;
            }
            else
            {
                Debug.LogError("Save file not found in : " + path);
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error Loading Save " + e);
            return null;
        }
    }
}
