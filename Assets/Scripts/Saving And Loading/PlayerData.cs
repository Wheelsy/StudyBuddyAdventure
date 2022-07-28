using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData 
{
    //Variables to store player data in
    public string gold;
    public string atk;
    public string def;
    public string initiative;
    public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    public List<string> inventoryKeys = new List<string>();
    public List<int> inventoryValues = new List<int>();
    public string equippedItem;
    public string equippedPotion;
    public int numTasks;
    public List<string> toDoValues = new List<string>();
    public List<bool> toDoCompleteds = new List<bool>();
    public List<int> unlockedBackgrounds = new List<int>();
    public List<int> unlockedSongs = new List<int>();
    public int curBg;
    public string date;
    public string numAdsAvailable;
    public bool htpBtnClicked;

    public PlayerData(CharacterPanel cp, Inventory inv,ItemManager im, ToDo td, BackgroundSelector bg, AudioManager am, DailyAd da, GameMaster gm)
    {
        htpBtnClicked = gm.HtpBtnClicked;
        date = da.Date;
        numAdsAvailable = da.numDailyAdsAvailable.text;
        curBg = bg.CurIndex;
        gold = cp.Gold.text.ToString();
        atk = cp.Atk.text.ToString();
        def = cp.Def.text.ToString();
        initiative = cp.Initiative.text.ToString();
        numTasks = td.GetNumTasksToSave();
        toDoValues = td.GetTaskValueToSave();
        toDoCompleteds = td.GetTaskCompletedToSave();

        for(int i = 0; i < bg.Locked.Length; i++)
        {
            if (!bg.Locked[i])
            {
                unlockedBackgrounds.Add(i);
            }
        }

        for (int i = 0; i < am.Locked.Length; i++)
        {
            if (!am.Locked[i])
            {
                unlockedSongs.Add(i);
            }
        }

        foreach (string key in inv.InvItemsKey)
        {
            if (inv.InvItemsValue[inv.InvItemsKey.IndexOf(key)] > 0)
            {
                Item i = im.GetItemByNameMatch(key);
                inventoryKeys.Add(i.name);
                inventoryValues.Add(inv.InvItemsValue[inv.InvItemsKey.IndexOf(key)]);
            }
        }

        if (im.GetItemBySpriteMatch(cp.SetSlot.sprite) != null)
        {
            equippedItem = im.GetItemBySpriteMatch(cp.SetSlot.sprite).name;
        }
        if (im.GetItemBySpriteMatch(cp.PotionSlot.sprite) != null)
        {
            equippedPotion = im.GetItemBySpriteMatch(cp.PotionSlot.sprite).name; 
        }
    }
}
