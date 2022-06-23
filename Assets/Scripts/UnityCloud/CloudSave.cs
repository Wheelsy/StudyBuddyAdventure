using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using TMPro;

public class CloudSave : MonoBehaviour
{
    public static async void Save(CharacterPanel cp, Inventory inv, ItemManager im, ToDo td, BackgroundSelector bg, AudioManager am)
    {
        //PlayerData localData = new PlayerData(cp, inv, im, td, bg, am);
        PlayerData data = new PlayerData(cp, inv, im, td, bg, am);
        var dataToCloud = new Dictionary<string, object>();
        dataToCloud["saveData"] = data;

        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(dataToCloud);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
