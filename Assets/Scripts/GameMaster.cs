using TMPro;
using Unity.Services.Authentication;
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
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingError;

    private bool noInternet = false;
    private bool htpBtnClicked = false;

    public bool HtpBtnClicked { get => htpBtnClicked; set => htpBtnClicked = value; }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void FixedUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (noInternet == false)
            {
                noInternet = true;
            }

            if (!loadingScreen.activeInHierarchy)
            {
                loadingScreen.SetActive(true);
                loadingError.gameObject.SetActive(true);
                loadingError.text = "Error: No internet connection.";
            }
        }
        else
        {
            if (noInternet == true)
            {
                loadingScreen.SetActive(false);
                loadingError.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (htpBtnClicked && howToPlayBtn.GetComponent<Image>().sprite == howToPlayRed)
        {
            howToPlayBtn.GetComponent<Image>().sprite = howToPlayNormal;
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

    private void OnApplicationFocus()
    {
        if(!loadingScreen.activeInHierarchy)SaveData();
    }

    public static void SaveData()
    {
        Debug.Log("saving data");
        //Grabbing game object in scene required to pass to the cloud saving script
        DailyAd da = GameObject.Find("AdManager").GetComponent<DailyAd>();
        CharacterPanel cp = GameObject.Find("LoadoutContainer").GetComponent<CharacterPanel>();
        Inventory inv = GameObject.Find("LoadoutContainer").GetComponent<Inventory>();
        ItemManager im = GameObject.Find("GameMaster").GetComponent<ItemManager>();
        ToDo td = GameObject.Find("ToDoListContainer").GetComponent<ToDo>();
        BackgroundSelector bg = GameObject.Find("ShopsContainer").GetComponent<BackgroundSelector>();
        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GameMaster gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        //Saving daily ad player prefs here before cloud saving
        PlayerPrefs.Save();

        CloudSave.Save(cp, inv, im, td, bg, am, da, gm);
    }
}
