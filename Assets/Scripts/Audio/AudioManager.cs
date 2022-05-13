using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] shopSongs;
    [SerializeField]
    private GameObject[] shopSongButtons;
    [SerializeField]
    private List <AudioClip> ownedSongs;
    [SerializeField]
    private TextMeshProUGUI[] songNames;
    [SerializeField]
    private Image[] locks;
    [SerializeField]
    private Sprite[] lockUnlock;
    [SerializeField]
    private TextMeshProUGUI cost;
    [SerializeField]
    private TextMeshProUGUI availableGold;
    [SerializeField]
    private bool[] locked;

    public GameObject buySongView;
    public CharacterPanel cp;

    private int curIndex;
    private AudioSource freeSong;
    //Track which song in the array is currently playing
    private AudioSource currentSong;

    //Stores whether the song has been purchased or not
    public bool[] Locked { get => locked; set => locked = value; }
    //Index of what the current song is. Need this for loading the song on game start
    public int CurIndex { get => curIndex; set => curIndex = value; }

    private void Start()
    {
        //populate the names of the songs in the shop
        for (int i = 0; i < shopSongs.Length; i++)
        {
            songNames[i].text = shopSongs[i].name;
        }
    }

    //Play the song at the index which has been passed in.
    //Stop playing the old song and update the current song index
    public void PlaySongFromShop(int index)
    {
        if (!Locked[index])
        {
            currentSong.Stop();
            currentSong.clip = ownedSongs[index];
            currentSong.Play();
        }
        else
        {
            availableGold.text = cp.Gold.text.ToString();
            cost.text = shopSongButtons[index].GetComponent<GoldPrice>().Price.ToString();
            buySongView.SetActive(true);
        }
    }

    //Check if the player has enough money to puchase the background
    //If so unlock it and set it as the new default background
    public void BuySong()
    {
        int price = int.Parse(cost.text);
        int curGold = int.Parse(availableGold.text);
        if (curGold >= price)
        {
            UnlockSong(CurIndex);
            cp.Gold.text = (curGold - price).ToString();
            ownedSongs.Add(shopSongs[curIndex]);
        }
    }

    //Set the song to unlocked
    //Set lock image to unlocked
    //Add song to owned songs
    public void UnlockSong(int index)
    {
        Locked[index] = false;
        locks[index].sprite = lockUnlock[1];
        buySongView.SetActive(false);
    }
}
