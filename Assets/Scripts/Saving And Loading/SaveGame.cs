using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveGame
{
    public static void Save( CharacterPanel cp, Inventory inv, ItemManager im, ToDo td, BackgroundSelector bg)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //set save file location
        string path = Application.persistentDataPath + "/player.savedata";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(cp, inv, im, td, bg);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    //deserialize saved data
    public static PlayerData Load()
    {
        try
        {
            string path = Application.persistentDataPath + "/player.savedata";

            //if there is a save file continue
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

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
