using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestResolution : MonoBehaviour
{
    [SerializeField]
    private Buddy buddy;
    [SerializeField]
    private ItemManager im;
    [SerializeField]
    private GameObject questOutcome;
    [SerializeField]
    private GameObject set;
    [SerializeField]
    private GameObject potion;
    [SerializeField]
    private TextMeshProUGUI questName;
    [SerializeField]
    private TextMeshProUGUI outcome;
    [SerializeField]
    private TextMeshProUGUI events;
    [SerializeField]
    private TextMeshProUGUI goldValue;
    [SerializeField]
    private CharacterPanel cp;
    [SerializeField]
    private GameObject addSetToInvButton;
    [SerializeField]
    private GameObject addPotionToInvButton;
    private int atk;
    private int def;
    private int initiative;
    private QuestManager qm;


    private void Start()
    {
        qm = gameObject.GetComponent<QuestManager>();
    }

    //If inititiative is higher than compare buddy atk to quest def
    //Else compare buddy def to quest atk
    //Call success or failure methods based on outcome
    public void ResolveQuest()
    {
        qm.ResetQuests();//Change the available quests
        string difficulty = qm.ActiveQuest.Difficulty;
        CalulateQuestStrength(difficulty);

        float playerInit = buddy.Initiative;
        float playerAtk = buddy.Atk;
        float playerDef = buddy.Def;
        int durability = 0;
        string potionType = null;

        //Apply potion effects
        if (!cp.PotionSlot.sprite.name.Equals("empty"))
        {
            potionType = im.GetItemBySpriteMatch(cp.PotionSlot.sprite).potion.ToString();
            switch (potionType)
            {
                case "OneUp":
                    break;
                case "DoubleAtk":
                    playerAtk *= 2;
                    cp.RemovePotion();
                    break;
                case "DoubleDef":
                    playerDef *= 2;
                    cp.RemovePotion();
                    break;
                case "DoubleInit":
                    playerInit *= 2;
                    cp.RemovePotion();
                    break;
            }
        }

        //Check if one up potion is required ie quest failed
        //If used exhaust the potion
        if (potionType != null)
        {
            if (potionType.Equals("OneUp"))
            {
                if (playerInit >= initiative)
                {
                    if (playerAtk < def)
                    {
                        CalulateQuestStrength(difficulty);
                        cp.RemovePotion();
                    }
                }
                else
                {
                    if (playerDef < atk)
                    {
                        CalulateQuestStrength(difficulty);
                        cp.RemovePotion();
                    }
                }
            }
        }

        Debug.Log("Quest Atk: " + atk);
        Debug.Log("Quest Def: " + def);
        //Compare player vs quest stats
        if (playerInit >= initiative)
        {
            if (playerAtk > def)
            {
                QuestSuccess(difficulty);
            }
            else
            {
                QuestFailed(difficulty);                
            }
        }
        else
        {
            if (playerDef >= atk)
            {
                QuestSuccess(difficulty);
            }
            else
            {
                QuestFailed(difficulty);
            }
        }

        //Calculate whether the set was destroyed during quest
        Item set = im.GetItemBySpriteMatch(cp.SetSlot.sprite);
        int weaponBreakChance = Random.Range(0,101);
        if (set != null)
        {
            durability = set.durability;
        }
        if(weaponBreakChance - durability >= 90)
        {
            if (!cp.SetSlot.name.Equals("empty"))//Incase there is no equipped set we check for null
            {
                cp.AddOrSubtractAtk(-set.atk);
                cp.AddOrSubtractDef(-set.def);
                cp.AddOrSubtractInitiative(-set.initiative);
                cp.AddOrSubtractDurability(-set.durability);
                cp.RemoveSet();
            }
        }
        buddy.ReturnFromQuest();
    }

    //Generate quest stats
    private void CalulateQuestStrength(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                atk = Random.Range(0, 10);
                def = Random.Range(0, 10);
                initiative = Random.Range(0, 10);
                break;

            case "Medium":
                atk = Random.Range(5, 15);
                def = Random.Range(5, 15);
                initiative = Random.Range(5, 15);
                break;

            case "Hard":
                atk = Random.Range(10, 25);
                def = Random.Range(10, 25);
                initiative = Random.Range(10, 25);
                break;

            case "Extreme":
                atk = Random.Range(15, 40);
                def = Random.Range(15, 40);
                initiative = Random.Range(15, 40);
                break;
        }
    }

    //If drop chance passed => Find a reward to display and add gold to player bank
    private void QuestSuccess(string difficulty)
    {
        
        bool pDrop = DoesPotionDrop();
        bool sDrop = DoesSetDrop();
        Sprite setSprite = null;
        Sprite potionSprite = null;
        Item s = null;
        Item p = null;
        events.text = qm.ActiveQuest.Victory;
        string dropType = CalculateDropType(difficulty);

        switch (dropType)
        {
            case "Easy":
                int eSetDrop = Random.Range(0, im.EasyRewards.Count);
                int ePotionDrop = Random.Range(0, im.Potions.Count);
                if (sDrop)
                {
                    Debug.Log("dropping set because sdrop = " + sDrop);
                    s = (Item)im.GetReward(difficulty, eSetDrop);
                    setSprite = s.itemArt;
                }

                if (pDrop)
                {
                    p = (Item)im.GetPotion(ePotionDrop);
                    potionSprite = p.itemArt;
                }
                break;

            case "Medium":
                int mSetDrop = Random.Range(0, im.MediumRewards.Count);
                int mPotionDrop = Random.Range(0, im.Potions.Count);
                if (sDrop)
                {
                    s = (Item)im.GetReward(difficulty, mSetDrop);
                    setSprite = s.itemArt;
                }

                if (pDrop)
                {
                    p = (Item)im.GetPotion(mPotionDrop);
                    potionSprite = p.itemArt;
                }
                break;

            case "Hard":
                int hSetDrop = Random.Range(0, im.HardRewards.Count);
                int hPotionDrop = Random.Range(0, im.Potions.Count);
                if (sDrop)
                {
                    s = (Item)im.GetReward(difficulty, hSetDrop);
                    setSprite = s.itemArt;
                }

                if (pDrop)
                {
                    p = (Item)im.GetPotion(hPotionDrop);
                    potionSprite = p.itemArt;
                }
                break;

            case "Extreme":
                int xSetDrop = Random.Range(0, im.ExtremeRewards.Count);
                int xPotionDrop = Random.Range(0, im.Potions.Count);
                if (sDrop)
                {
                    s = (Item)im.GetReward(difficulty, xSetDrop);
                    setSprite = s.itemArt;
                }

                if (pDrop)
                {
                    p = (Item)im.GetPotion(xPotionDrop);
                    potionSprite = p.itemArt;
                }
                break;
        }

        goldValue.text = qm.ActiveQuest.Reward;
        int gold = int.Parse(qm.ActiveQuest.Reward);
        cp.AddOrSubtractGold(gold);
        questOutcome.SetActive(true);
        outcome.text = "Success";
        questName.text = qm.ActiveQuest.Name;
        if (sDrop)
        {
            set.GetComponent<Image>().sprite = setSprite;
        }
        if (pDrop)
        {
            potion.GetComponent<Image>().sprite = potionSprite;
        }
    }

    private void QuestFailed(string difficulty)
    {
        events.text = qm.ActiveQuest.Defeat;
        questOutcome.SetActive(true);
        outcome.text = "Defeat";
        questName.text = qm.ActiveQuest.Name;
    }

    private bool DoesSetDrop()
    {
        int setDropChance = Random.Range(1, 100);
        bool result = false;

        if (setDropChance <= 80)
        {
            addSetToInvButton.SetActive(true);
            result = true;
        }
        else
        {
            addSetToInvButton.SetActive(false);
            result = false;
        }

        return result;
    }

    private bool DoesPotionDrop()
    {
        int potionDropChance = Random.Range(1, 100);
        bool result = false;

        if (potionDropChance <= 65)
        {
            addPotionToInvButton.SetActive(true);
            result = true;
        }
        else
        {
            addPotionToInvButton.SetActive(false);
            result = false;
        }

        return result;
    }

    private string CalculateDropType(string difficulty)
    {
        string dropType = null;
        int dropBonus = 0;
        switch (difficulty)
        {
            case "Medium":
                dropBonus = 5;
                break;

            case "Hard":
                dropBonus = 10;
                break;

            case "Extreme":
                dropBonus = 20;
                break;
        }

        int dropRoll = Random.Range(0, 101);
        int dropTotal = dropBonus + dropRoll;

        if(dropTotal <= 60)
        {
            dropType = "Easy";
        }
        else if(dropTotal > 60 && dropTotal <= 80)
        {
            dropType = "Medium";
        }
        else if(dropTotal > 80 && dropTotal <= 95)
        {
            dropType = "Hard";
        }
        else
        {
            dropType = "Extreme";
        }

        return dropType;
    }
}
