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
    private TextMeshProUGUI[] shopSongNames;
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
    //Track which song in the array is currently playing
    [SerializeField]
    private AudioSource currentSong;

    public GameObject buySongView;
    public CharacterPanel cp;
    public GameObject snippetTxt;
    public ShopSwitcher sw;
    public TextMeshProUGUI buySongName;

    private MusicLibrary library;
    public static int curIndex;
    public static int curSnipIndex;
    private AudioSource freeSong;

    //Stores whether the song has been purchased or not
    public bool[] Locked { get => locked; set => locked = value; }
    //Index of what the current song is. Need this for loading the song on game start
    public int CurIndex { get => curIndex; set => curIndex = value; }

    private void Awake()
    {
        library = gameObject.GetComponent<MusicLibrary>();    
    }

    private void Start()
    {
        //populate the names of the songs in the shop
        for (int i = 0; i < shopSongs.Length; i++)
        {
            shopSongNames[i].text = shopSongs[i].name;

        }
        library.AddToLibrary(shopSongs[0]);
    }

    //Play the song at the index which has been passed in.
    //Stop playing the old song and update the current song index
    public void ShopSongClicked(int index)
    {
        if (Locked[index])
        {
            curIndex = index;
            availableGold.text = cp.Gold.text.ToString();
            cost.text = shopSongButtons[index].GetComponent<GoldPrice>().Price.ToString();
            buySongName.text = shopSongNames[index].text;
            buySongView.SetActive(true);
        }
    }

    //Start the snippet coroutine
    //If there is a snipet already playing then stop that coroutine
    public void PlaySnipClicked(int index)
    {
        if (currentSong.isPlaying)
        {
            StopCoroutine("PlaySnippet");
        }

        curSnipIndex = index;
        StartCoroutine("PlaySnippet");
    }

    //Play the first 5 seconds of a song
    IEnumerator PlaySnippet()
    {
        if (currentSong != null)
        {
            currentSong.Stop();
        }

        snippetTxt.SetActive(true);
        currentSong.clip = shopSongs[curSnipIndex];
        currentSong.Play();

        yield return new WaitForSeconds(5);

        snippetTxt.SetActive(false);
        currentSong.Stop();
        currentSong.clip = null;
    }

    //Check if the player has enough money to puchase the background
    //If so unlock it.
    public void BuySong()
    {
        int price = int.Parse(cost.text);
        int curGold = int.Parse(availableGold.text);
        if (curGold >= price)
        {
            UnlockSong(CurIndex);
            cp.Gold.text = (curGold - price).ToString();
            ownedSongs.Add(shopSongs[curIndex]);
            library.AddToLibrary(shopSongs[curIndex]);
            GameMaster.SaveData();
        }
    }

    public void LoadSong(int index)
    {
        if (!shopSongs[index].name.Equals("November Snow"))
        {
            UnlockSong(index);
            library.AddToLibrary(shopSongs[index]);
        }
    }

    //Close buy song menu
    public void CancelSongPurchase()
    {
        buySongView.SetActive(false);
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
