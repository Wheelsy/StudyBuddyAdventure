using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

public class CloudLoad : MonoBehaviour
{
    [SerializeField]
    private Image[] checkboxes;

    private Inventory inv;
    private CharacterPanel cp;
    private ItemManager im;
    private Buddy buddy;
    private ToDo td;
    private BackgroundSelector bg;
    private AudioManager am;
    private PetManager pm;
    private DailyAd da;

    public GameObject loadScreen;
    public TextMeshProUGUI playerId;

    private void Awake()
    {
        cp = GameObject.Find("LoadoutContainer").GetComponent<CharacterPanel>();
        inv = GameObject.Find("LoadoutContainer").GetComponent<Inventory>();
        im = GameObject.Find("GameMaster").GetComponent<ItemManager>();
        buddy = GameObject.Find("StudyBuddy").GetComponent<Buddy>();
        td = GameObject.Find("ToDoListContainer").GetComponent<ToDo>();
        bg = GameObject.Find("ShopsContainer").GetComponent<BackgroundSelector>();
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        pm = GameObject.Find("GameMaster").GetComponent<PetManager>();
        da = GameObject.Find("AdManager").GetComponent<DailyAd>();
    }

    public async void CheckForLoadData()
    {
        try
        {
            List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

            if (keys.Contains("saveData"))
            {
                Load(cp, inv, im, td, bg, am, buddy, pm, da);
            }
            else
            {
                Debug.Log("No data to load");
                da.Setup();
            }
        }
        catch(Exception e)
        {
            gameObject.GetComponent<Initialise>().ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = e.ToString();
            gameObject.GetComponent<Initialise>().ConnectionErrorText.SetActive(true);
        }
    }

    public async void Load(CharacterPanel cp, Inventory inv, ItemManager im, ToDo td, BackgroundSelector bg, AudioManager am, Buddy buddy, PetManager pm, DailyAd da)
    {
        try
        {
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "saveData" });
            PlayerData data = JsonUtility.FromJson<PlayerData>(savedData["saveData"]);

            Debug.Log("loading num ads available: " + data.numAdsAvailable);
            da.Date = data.date;
            da.numDailyAdsAvailable.text = data.numAdsAvailable;
            cp.Atk.text = data.atk;
            cp.Def.text = data.def;
            cp.Initiative.text = data.initiative;
            cp.Gold.text = data.gold;

            foreach (int i in data.unlockedBackgrounds)
            {
                bg.UnlockBackground(i);
            }

            bg.LoadBackground(data.curBg);

            foreach (int i in data.unlockedSongs)
            {
                am.LoadSong(i);
            }

            for (int i = 0; i < data.numTasks; i++)
            {
                td.tasks[i].SetActive(true);
                td.tasks[i].GetComponentInChildren<TextMeshProUGUI>().text = data.toDoValues[i].ToString();
                if (data.toDoCompleteds[i])
                {
                    td.CompleteTask(i, checkboxes[i]);
                }
            }

            if (data.inventoryKeys != null)
            {
                for (int i = 0; i < data.inventoryKeys.Count; i++)
                {
                    string key = data.inventoryKeys[i].ToString();
                    key.TrimEnd();
                    key.TrimStart();
                    if (key != "")
                    {
                        inv.InvItemsKey.Add(key);
                        inv.InvItemsValue.Add(data.inventoryValues[i]);
                    }
                }
            }

            foreach (string key in inv.InvItemsKey)
            {
                Item i = im.GetItemByNameMatch(key);
                if (i != null)
                {
                    Image slot = inv.FindEmptySlot(i.itemArt);
                    slot.sprite = i.itemArt;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = inv.InvItemsValue[inv.InvItemsKey.IndexOf(key)].ToString();
                }
            }

            if (data.equippedPotion.Trim() != "")
            {
                Item potion = im.GetItemByNameMatch(data.equippedPotion);
                if (potion != null)
                {
                    cp.UpdatePotionImage(potion.itemArt);
                }
            }

            if (data.equippedItem.Trim() != "")
            {
                Item item = im.GetItemByNameMatch(data.equippedItem);
                if (item != null)
                {
                    if (item.hasPet)
                    {
                        pm.TurnOnPet(item.petIndex);
                    }
                    cp.UpdateSetImage(item.itemArt);
                    cp.UpdateCharacterImage(item.skin[0]);
                    buddy.UpdateCurrentSkin(item.skin[0], item.skin[1]);
                }
            }
        }
        finally
        {
            da.Setup();
        }
    }
}
