using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackgroundSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] backgrounds;
    [SerializeField]
    private bool[] locked;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Image buyBackgroundImg;
    [SerializeField]
    private TextMeshProUGUI cost;
    [SerializeField]
    private TextMeshProUGUI availableGold;
    [SerializeField]
    private Sprite[] lockUnlock;

    private int tmpIndex;
    private int curIndex = 0; 

    public CharacterPanel cp;
    public GameObject buyBackgroundView;
    public ShopSwitcher sw;

    //Stores whether the background has been purchased or not
    public bool[] Locked { get => locked; set => locked = value; }
    //Index of what the current background is. Need this for loading the backround on game start
    public int CurIndex { get => curIndex; set => curIndex = value; }

    //Takes in the index for the backgrounds array
    //If that background has already been unlocked then set it as the new background
    //If not open the buy background menu
    public void SetBackground(int index)
    {
        Debug.Log("setting bg");
        if (!Locked[index])
        {
            background.GetComponent<SpriteRenderer>().sprite = backgrounds[index].GetComponent<Image>().sprite;
            SetCurrentBackgroundMarker(index);
            curIndex = index;
        }
        else
        {
            tmpIndex = index;
            availableGold.text = cp.Gold.text.ToString();
            cost.text = backgrounds[index].GetComponent<GoldPrice>().Price.ToString();
            buyBackgroundView.SetActive(true);
            buyBackgroundImg.sprite = backgrounds[index].GetComponent<Image>().sprite;
        }
    }

    //Check if the player has enough money to puchase the background
    //If so unlock it and set it as the new default background
    public void BuyBackground()
    {
        int price = int.Parse(cost.text);
        int curGold = int.Parse(availableGold.text);
        if(curGold >= price)
        {
            UnlockBackground(tmpIndex);
            cp.Gold.text = (curGold - price).ToString();
            GameMaster.SaveData();
        }
    }

    //Set the background to unlocked
    //Set lock image to unlocked
    //Turn off buy background view
    public void UnlockBackground(int index)
    {
        Locked[index] = false;
        backgrounds[index].transform.GetChild(1).GetComponent<Image>().sprite = lockUnlock[1];
        buyBackgroundView.SetActive(false);      
    }

    //Load the default background on game start
    public void LoadBackground(int index)
    {
        background.GetComponent<SpriteRenderer>().sprite = backgrounds[index].GetComponent<Image>().sprite;
        SetCurrentBackgroundMarker(index);
        curIndex = index;
    }


    //Close buy background menu
    public void CancelBackgroundPurchase()
    {
        buyBackgroundView.SetActive(false);
    }

    //Add the gem logo to the background which is currently the default
    public void SetCurrentBackgroundMarker(int index)
    {
        foreach(GameObject g in backgrounds)
        {
            g.transform.GetChild(2).GetComponent<Image>().enabled = false;
        }

        backgrounds[index].transform.GetChild(2).GetComponent<Image>().enabled = true;
    }
}
