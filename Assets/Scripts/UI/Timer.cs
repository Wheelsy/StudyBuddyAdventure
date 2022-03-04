using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private QuestResolution qr;
    private TextMeshProUGUI text;
    [SerializeField]
    private float seconds = 60;
    [SerializeField]
    private float minutes;
    private bool startTimer = false;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(int time)
    {
        minutes = time;
        startTimer = true;
        text.text = time.ToString();
    }

    //While timer value > 0 minus time
    //When it hits 0 resolve the quest
    private void Update()
    {
        if(minutes > 0 || seconds > 0 && startTimer) 
        {
            seconds -= Time.deltaTime;
            DisplayTime();
        }
        else if(minutes <= 0 && seconds <= 0 && startTimer)
        {
            startTimer = false;
            text.text = "";
            qr.ResolveQuest();
        }
    }

    private void DisplayTime()
    {
        if(seconds <= 0)
        {
            minutes -= 1;
            if (minutes > 0)
            {
                seconds = 60;
            }
        }
        text.text = minutes + ":" + seconds.ToString("N0");
    }
}
