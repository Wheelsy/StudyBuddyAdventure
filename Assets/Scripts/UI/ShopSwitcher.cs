using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSwitcher : MonoBehaviour
{
    public GameObject[] shops;
    public string[] shopNames;
    public GameObject previousShopBtn;
    public GameObject nextShopBtn;
    public TextMeshProUGUI title;

    private int curShop = 0;

    public void ResetTitleText()
    {
        title.text = shopNames[0];
        curShop = 0;
    }

    //Move to the next shop in the array
    public void NextShop()
    {
        if (curShop + 1 <= shops.Length)
        {
            shops[curShop].SetActive(false);
            shops[curShop + 1].SetActive(true);
            title.text = shopNames[curShop + 1];
            curShop++;
        }
    }

    //Move to the previous shop
    public void PreviousShop()
    {
        if(curShop - 1 >= 0)
        {
            shops[curShop].SetActive(false);
            shops[curShop - 1].SetActive(true);
            title.text = shopNames[curShop - 1];
            curShop--;
        }
    }
}
