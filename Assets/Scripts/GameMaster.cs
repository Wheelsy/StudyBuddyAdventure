using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    private Inventory inv;
    private CharacterPanel cp;
    private ItemManager im;
    private Buddy buddy;
    private ToDo td;
    private BackgroundSelector bg;
    private PetManager pm;
    [SerializeField]
    private Image[] checkboxes;

    private void Awake()
    {
        cp = GameObject.Find("LoadoutContainer").GetComponent<CharacterPanel>();
        inv = GameObject.Find("LoadoutContainer").GetComponent<Inventory>();
        im = gameObject.GetComponent<ItemManager>();
        buddy = GameObject.Find("StudyBuddy").GetComponent<Buddy>();
        td = GameObject.Find("ToDoListContainer").GetComponent<ToDo>();
        bg = GameObject.Find("ShopsContainer").GetComponent<BackgroundSelector>();
        pm = gameObject.GetComponent<PetManager>();
    }

    private void Start()
    {
        im = gameObject.GetComponent<ItemManager>();

        //if a save file exists the call load method
        if (File.Exists(Application.persistentDataPath + "/player.savedata"))
        {
            LoadData();
        }
    }

    public static void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/player.savedata";
        File.Delete(path);
    }

    public void LoadData()
    {
        PlayerData data = SaveGame.Load();
        cp.Atk.text = data.atk;
        cp.Def.text = data.def;
        cp.Initiative.text = data.initiative;
        cp.Gold.text = data.gold;

        foreach(int i in data.unlockedBackgrounds)
        {
            bg.UnlockBackground(data.unlockedBackgrounds[i]);
        }

        bg.LoadBackground(data.curBg);

        for(int i = 0; i < data.numTasks; i++)
        {
            td.tasks[i].SetActive(true);
            td.tasks[i].GetComponentInChildren<TextMeshProUGUI>().text = data.toDoValues[i];
            if (data.toDoCompleteds[i])
            {
                td.CompleteTask(i, checkboxes[i]);
            }
        }

        foreach(KeyValuePair<string, int> entry in data.inventoryItems)
        {
            Item i = im.GetItemByNameMatch(entry.Key);
            inv.SlotsInUse.Add(i.name, entry.Value);
        }

        foreach (KeyValuePair<string, int> entry in inv.SlotsInUse)
        {           
            Item i = im.GetItemByNameMatch(entry.Key);
            Image slot = inv.FindEmptySlot(i.itemArt);
            slot.sprite = i.itemArt;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = entry.Value.ToString();
        }

        if (data.equippedPotion != null)
        {
            Item potion = im.GetItemByNameMatch(data.equippedPotion);
            cp.UpdatePotionImage(potion.itemArt);
        }

        if (data.equippedItem != null)
        {
            Item item = im.GetItemByNameMatch(data.equippedItem);
            if (item.hasPet)
            {
                pm.TurnOnPet(item.petIndex);
            }
            cp.UpdateSetImage(item.itemArt);
            cp.UpdateCharacterImage(item.skin[0]);
            buddy.UpdateCurrentSkin(item.skin[0], item.skin[1]);
        }
    }

    public static void SaveData()
    {
        CharacterPanel cp = GameObject.Find("LoadoutContainer").GetComponent<CharacterPanel>();
        Inventory inv = GameObject.Find("LoadoutContainer").GetComponent<Inventory>();
        ItemManager im = GameObject.Find("GameMaster").GetComponent<ItemManager>();
        ToDo td = GameObject.Find("ToDoListContainer").GetComponent<ToDo>();
        BackgroundSelector bg = GameObject.Find("ShopsContainer").GetComponent<BackgroundSelector>();

        SaveGame.Save(cp, inv, im, td, bg);
    }
}
