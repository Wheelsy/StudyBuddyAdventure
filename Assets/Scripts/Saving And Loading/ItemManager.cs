using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //Storage for all the items in the game
    [SerializeField]
    private List<ScriptableObject> easyRewards = new List<ScriptableObject>();
    [SerializeField]
    private List<ScriptableObject> mediumRewards = new List<ScriptableObject>();
    [SerializeField]
    private List<ScriptableObject> hardRewards = new List<ScriptableObject>();
    [SerializeField]
    private List<ScriptableObject> extremeRewards = new List<ScriptableObject>();
    [SerializeField]
    private List<ScriptableObject> potions = new List<ScriptableObject>();

    private List<ScriptableObject> allItems = new List<ScriptableObject>();

    public List<ScriptableObject> EasyRewards { get => easyRewards; set => easyRewards = value; }
    public List<ScriptableObject> MediumRewards { get => mediumRewards; set => mediumRewards = value; }
    public List<ScriptableObject> HardRewards { get => hardRewards; set => hardRewards = value; }
    public List<ScriptableObject> ExtremeRewards { get => extremeRewards; set => extremeRewards = value; }
    public List<ScriptableObject> Potions { get => potions; set => potions = value; }

    //Combine all items into 1 list
    private void Awake()
    {        
        allItems.AddRange(easyRewards);
        allItems.AddRange(mediumRewards);
        allItems.AddRange(hardRewards);
        allItems.AddRange(extremeRewards);
        allItems.AddRange(potions);
    }

    //Return a reward from the passed difficulty group
    public ScriptableObject GetReward(string difficulty, int index)
    {
        switch (difficulty)
        {
            case "Easy":
                return EasyRewards[index];

            case "Medium":
                return MediumRewards[index];

            case "Hard":
                return HardRewards[index];

            case "Extreme":
                return ExtremeRewards[index];

            default:
                return null;
        }
    }

    //Return potion at passd index 
    public ScriptableObject GetPotion(int index)
    {
        return potions[index];
    }

    //Search for a matching sprite amongst all items and if found return the scriptable obj
    //If not found return null
    public Item GetItemBySpriteMatch(Sprite sprite)
    {
        foreach(Item s in allItems)
        {
            if(s.itemArt == sprite)
            {
                return s;
            }
        }

        return null;
    }

    public Item GetItemByNameMatch(string itemName)
    {
        foreach (Item s in allItems)
        {
            if (s.name.Equals(itemName))
            {
                return s;
            }
        }
        return null;
    }
}
