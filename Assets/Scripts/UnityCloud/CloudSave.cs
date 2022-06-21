using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSave : MonoBehaviour
{
   public async void Save(CharacterPanel cp, Inventory inv, ItemManager im, ToDo td, BackgroundSelector bg, AudioManager am)
    {
        PlayerData localData = new PlayerData(cp, inv, im, td, bg, am);
        var dataToCloud = new Dictionary<string, object> { { "key", "someValue" } };

        if (localData.gold != null)
        {
            dataToCloud["key"] = localData.gold;
            await CloudSaveService.Instance.Data.ForceSaveAsync(dataToCloud);
        }
    }
}
