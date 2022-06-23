using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    private Image[] checkboxes;

    public GameObject howToPlayBtn;
    public Sprite howToPlayNormal;
    public Sprite howToPlayRed;
    public CloudSave cloud;
    public TextMeshProUGUI playerId;
    public GameObject deleteAccount;
    public GameObject internetErrorText;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("playCount"))
        {
            PlayerPrefs.SetInt("playCount", 1);
            InvokeRepeating("FlashIcon", 0, 0.75f);
        }
        else
        {
            PlayerPrefs.SetInt("playCount", PlayerPrefs.GetInt("playCount") + 1);
        }      
        PlayerPrefs.Save();
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void FixedUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!internetErrorText.activeInHierarchy)
            {
                internetErrorText.SetActive(true);
            }
        }
        else
        {
            if (internetErrorText.activeInHierarchy)
            {
                internetErrorText.SetActive(false);
            }
        }
    }

    public void DeleteAccountClicked()
    {
        deleteAccount.SetActive(true);
    }

    public async void DeleteAccount()
    {
        try
        {
            await AuthenticationService.Instance.DeleteAccountAsync();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);

        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetPlayCount()
    {
        PlayerPrefs.DeleteKey("playCount");
        PlayerPrefs.Save();
    }

    private void FlashIcon()
    {
        if(howToPlayBtn.GetComponent<Image>().sprite == howToPlayNormal)
        {
            howToPlayBtn.GetComponent<Image>().sprite = howToPlayRed;
        }
        else
        {
            howToPlayBtn.GetComponent<Image>().sprite = howToPlayNormal;
        }
    }

    public void CancelFlashIcon()
    {
        CancelInvoke("FlashIcon");
        howToPlayBtn.GetComponent<Image>().sprite = howToPlayNormal;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public static void SaveData()
    {
        CharacterPanel cp = GameObject.Find("LoadoutContainer").GetComponent<CharacterPanel>();
        Inventory inv = GameObject.Find("LoadoutContainer").GetComponent<Inventory>();
        ItemManager im = GameObject.Find("GameMaster").GetComponent<ItemManager>();
        ToDo td = GameObject.Find("ToDoListContainer").GetComponent<ToDo>();
        BackgroundSelector bg = GameObject.Find("ShopsContainer").GetComponent<BackgroundSelector>();
        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        CloudSave.Save(cp, inv, im, td, bg, am);
    }
}
