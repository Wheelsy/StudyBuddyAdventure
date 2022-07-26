using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DailyAd : MonoBehaviour
{
    public RewardedAdsButton rAB;
    public TextMeshProUGUI numDailyAdsAvailable;

    private string date;

    public string Date { get => date; set => date = value; }

    // Start a reccuring check for if daily ad has been watched
    public void Setup()
    {
        if(date == null || date.Equals(""))
        {
            date = DateTime.Today.ToString();
        }

        if (numDailyAdsAvailable.text == null || numDailyAdsAvailable.text.Equals(""))
        {
            numDailyAdsAvailable.text = "5";
        }

        if(rAB.GetComponent<Button>().interactable == false)
        {
            rAB.GetComponent<Button>().interactable = true;
        }
        InvokeRepeating("CheckDay", 1, 60);
    }

    //Check the date and compare it to the last date stored
    //If its different to todays date then we can refresh the daily ad
    private void CheckDay()
    {
        if (date != null)
        {
            if (!date.Equals(DateTime.Today.ToString()))
            {
                numDailyAdsAvailable.text = "5";
                date = DateTime.Today.ToString();
            }
        }
    }

    public void SetAvailableAds(string amount)
    {
        numDailyAdsAvailable.text = amount;
    }
}
