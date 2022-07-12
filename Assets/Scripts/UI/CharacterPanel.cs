using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gold;
    [SerializeField]
    private TextMeshProUGUI atk;
    [SerializeField]
    private TextMeshProUGUI def;
    [SerializeField]
    private TextMeshProUGUI initiative;
    [SerializeField]
    private TextMeshProUGUI durability;
    [SerializeField]
    private Image setSlot;
    [SerializeField]
    private Image potionSlot;
    [SerializeField]
    private Image characterSlot;
    [SerializeField]
    private ItemManager im;
    [SerializeField]
    private Buddy buddy;
    [SerializeField]
    private Sprite emptySprite;

    public TextMeshProUGUI Gold { get => gold; set => gold = value; }
    public TextMeshProUGUI Atk { get => atk; set => atk = value; }
    public TextMeshProUGUI Def { get => def; set => def = value; }
    public TextMeshProUGUI Initiative { get => initiative; set => initiative = value; }
    public Image SetSlot { get => setSlot; set => setSlot = value; }
    public Image PotionSlot { get => potionSlot; set => potionSlot = value; }
    public TextMeshProUGUI Durability { get => durability; set => durability = value; }

    //Assign buddies base stats on start
    private void Awake()
    {
        Item curSet = im.GetItemBySpriteMatch(setSlot.sprite);

        atk.text = buddy.Atk.ToString();
        def.text = buddy.Atk.ToString();
        initiative.text = buddy.Initiative.ToString();
        if (curSet != null) {
            durability.text = curSet.durability.ToString();
        }
    }

    public void AddOrSubtractAtk(int value)
    {
        atk.text = (buddy.Atk + value).ToString();
    }

    public void AddOrSubtractDef(int value)
    {
        def.text = (buddy.Def + value).ToString();
    }

    public void AddOrSubtractInitiative(int value)
    {
        initiative.text = (buddy.Initiative + value).ToString();
    }

    public void AddOrSubtractDurability(int value)
    {
        initiative.text = (buddy.Initiative + value).ToString();
    }

    public void AddOrSubtractGold(int value)
    {
        gold.text = (int.Parse(gold.text) + value).ToString();
        GameMaster.SaveData();
    }

    public void UpdateSetImage(Sprite sprite)
    {
        setSlot.sprite = sprite;
    }

    public void UpdatePotionImage(Sprite sprite)
    {
        potionSlot.sprite = sprite;
    }

    public void UpdateCharacterImage(Sprite sprite)
    {
        characterSlot.sprite = sprite;
    }

    public void RemovePotion()
    {
        potionSlot.sprite = emptySprite;
        GameMaster.SaveData();
    }

    public void RemoveSet()
    {
        setSlot.sprite = emptySprite;
        characterSlot.sprite = buddy.noSkin[0];
        buddy.RemoveSet();
        GameMaster.SaveData();
    }

    public void ResetStats()
    {
        initiative.text = buddy.Initiative.ToString();
        atk.text = buddy.Atk.ToString();
        def.text = buddy.Def.ToString();
        durability.text = "0";
        GameMaster.SaveData();
    }
    //Find the scriptable object and add its stats to the character panel
    public void UpdateStats(Sprite sprite)
    {
        Item newItem = im.GetItemBySpriteMatch(sprite);
        atk.text =(buddy.Atk + newItem.atk).ToString();
        durability.text = newItem.durability.ToString();
        def.text = (buddy.Def + newItem.def).ToString();
        initiative.text = (buddy.Initiative + newItem.initiative).ToString();
        GameMaster.SaveData();
    }
}
