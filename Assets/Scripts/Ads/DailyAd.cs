using System;
using UnityEngine;
using TMPro;

public class DailyAd : MonoBehaviour
{
    public RewardedAdsButton rAB;
    public TextMeshProUGUI numDailyAdsAvailable;

    private string date;
    private int numDailyAdsWatched;

    public string Date { get => date; set => date = value; }
    public int NumDailyAdsWatched { get => numDailyAdsWatched; set => numDailyAdsWatched = value; }

    // Check if playerprefs have been created for daily ads
    // Start a reccuring check for if daily ad has been watched
    public void Setup()
    {
        InvokeRepeating("CheckDay", 1, 60);

        if(date == null || date.Equals(""))
        {
            date = DateTime.Today.ToString();
        }

        RefreshAvailableAds();
    }

    //Check the date and compare it to the last date stored
    //If its different to todays date then we can refresh the daily ad
    private void CheckDay()
    {
        if (date != null)
        {
            if (!date.Equals(DateTime.Today.ToString()))
            {
                Debug.Log("date doesnt match");
                numDailyAdsWatched = 0;
                date = DateTime.Today.ToString();
                RefreshAvailableAds();
            }
        }
    }

    public void RefreshAvailableAds()
    {
        numDailyAdsAvailable.text = (5 - numDailyAdsWatched).ToString();
    }
}
