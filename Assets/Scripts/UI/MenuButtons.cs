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
    private GameObject blackoutImg;
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject backgroundShop;
    [SerializeField]
    private GameObject goldShop;
    [SerializeField]
    private Image set;
    [SerializeField]
    private Image potion;
    [SerializeField]
    private Sprite empty;
    [SerializeField]
    private ShopSwitcher switcher;

    public void QuestsMenu()
    {
        blackoutImg.SetActive(true);
        TurnOffDialogue();
        questMenu.SetActive(true);
    }

    public void LoadoutMenu()
    {
        blackoutImg.SetActive(true);
        TurnOffDialogue();
        loadoutMenu.SetActive(true);
    }

    public void ShopMenu()
    {
        shopMenu.SetActive(true);
        blackoutImg.SetActive(true);
    }

    public void QuestView(TextMeshProUGUI questName)
    {
        blackoutImg.SetActive(false);
        TurnOffDialogue();
        TurnOffMainButtons();
        questMenu.SetActive(false);
        questView.SetActive(true);
        questView.GetComponent<QuestView>().LoadView(questName.text);
    }

    public void QuestAccepted(TextMeshProUGUI questName)
    {
        blackoutImg.SetActive(false);
        TurnOnMainButtons();
        questView.SetActive(false);
        qm.SetActiveQuest(questName.text);
        timer.StartTimer(questView.GetComponent<QuestView>().OpenQuest.Time);
        buddy.LeaveForQuest();
    }

    public void ToDo()
    {
        blackoutImg.SetActive(true);
        toDo.SetActive(true);
        TurnOffDialogue();
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
                break;

            case "questOutcome":
                set.sprite = empty;
                potion.sprite = empty;
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
                break;
        }
        blackoutImg.SetActive(false);
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
