using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject questMenu;
    [SerializeField]
    private GameObject questView;
    [SerializeField]
    private GameObject loadoutMenu;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private QuestManager qm;
    [SerializeField]
    private Buddy buddy;
    [SerializeField]
    private Inventory inv;
    [SerializeField]
    private GameObject questOutcome;
    [SerializeField]
    private GameObject[] mainScreenButtons;
    [SerializeField]
    private Dialogue dialogue;
    [SerializeField]
    private GameObject toDo;
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject backgroundShop;
    [SerializeField]
    private GameObject musicShop;
    [SerializeField]
    private GameObject goldShop;
    [SerializeField]
    private GameObject backgroundBuy;
    [SerializeField]
    private GameObject songBuy;
    [SerializeField]
    private Image set;
    [SerializeField]
    private Image potion;
    [SerializeField]
    private Sprite empty;
    [SerializeField]
    private ShopSwitcher switcher;
    [SerializeField]
    private GameObject musicLibrary;
    [SerializeField]
    private GameObject howToPlay;
    [SerializeField]
    private GameObject cancelQuest;

    public GameMaster gm;
    public GameObject invFull;

    public void QuestsMenu()
    {
        TurnOffDialogue();
        questMenu.SetActive(true);
    }

    public void LoadoutMenu()
    {
        TurnOffDialogue();
        loadoutMenu.SetActive(true);
    }

    public void ShopMenu()
    {
        shopMenu.SetActive(true);
    }

    public void QuestView(TextMeshProUGUI questName)
    {
        TurnOffDialogue();
        TurnOffMainButtons();
        questMenu.SetActive(false);
        questView.SetActive(true);
        questView.GetComponent<QuestView>().LoadView(questName.text);
    }

    public void QuestAccepted(TextMeshProUGUI questName)
    {
        TurnOnMainButtons();
        questView.SetActive(false);
        qm.SetActiveQuest(questName.text);
        timer.StartTimer(questView.GetComponent<QuestView>().OpenQuest.Time);
        buddy.LeaveForQuest();
    }

    public void ToDo()
    {
        toDo.SetActive(true);
        TurnOffDialogue();
    }

    public void MusicLibrary()
    {
        musicLibrary.SetActive(true);
    }

    public void HowToPlay()
    {
        if (PlayerPrefs.GetInt("playCount") == 1)
        {
            gm.CancelFlashIcon();
        }

        howToPlay.SetActive(true);
    }

    public void Back(string from)
    {
        switch (from)
        {
            case "questView":
                questMenu.SetActive(true);
                questView.SetActive(false);
                break;

            case "questMenu":
                questMenu.SetActive(false);
                break;

            case "loadout":
                loadoutMenu.SetActive(false);

                if (invFull.activeInHierarchy)
                {
                    invFull.SetActive(false);
                }
                break;

            case "questOutcome":
                inv.GetComponent<Inventory>().invFullTxt.SetActive(false);
                set.sprite = empty;
                potion.sprite = empty;
                qm.GetComponent<QuestResolution>().notes.text = "";
                questOutcome.SetActive(false);
                break;

            case "toDo":
                toDo.SetActive(false);
                break;

            case "shop":
                backgroundShop.SetActive(false);
                goldShop.SetActive(true);
                switcher.ResetTitleText();
                shopMenu.SetActive(false);
                musicShop.SetActive(false);
                songBuy.SetActive(false);
                backgroundBuy.SetActive(false);
                break;

            case "musicLibrary":
                musicLibrary.SetActive(false);
                break;

            case "howToPlay":
                howToPlay.SetActive(false);
                break;

            case "cancelQuest":
                cancelQuest.SetActive(false);
                break;
        }
        TurnOnMainButtons();
    }

    private void TurnOffMainButtons()
    {
        foreach(GameObject g in mainScreenButtons)
        {
            g.SetActive(false);
        }
    }

    private void TurnOnMainButtons()
    {
        foreach (GameObject g in mainScreenButtons)
        {
            g.SetActive(true);
        }
    }

    private void TurnOffDialogue()
    {
        dialogue.StopSpeech();
    }
}
