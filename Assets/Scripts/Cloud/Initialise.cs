using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Initialise : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerId;
    [SerializeField]
    private GameObject connectionErrorText;
    [SerializeField]
    private GameObject loginScreen;

    private CloudLoad load;

    public Button adButton;
    public GameMaster gm;
    public GameObject ConnectionErrorText { get => connectionErrorText; set => connectionErrorText = value; }

    internal async Task Awake()
    {
        load = gameObject.GetComponent<CloudLoad>();
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

        InvokeRepeating("CheckSignInWorked", 1, 1);
    }

    private async void Start()
    {
        await SignInAnonymously();
    }

    private async void CheckSignInWorked()
    {
        if (!playerId.text.Trim().Equals(""))
        {
            Invoke("TurnOffLoadScreen", 1.2f);
            CancelInvoke("CheckSignInWorked");

            if (!PlayerPrefs.HasKey("playCount"))
            {
                PlayerPrefs.SetInt("playCount", 1);
                gm.StartFlashIcon();
            }
            else if (PlayerPrefs.GetInt("playCount") <= 1)
            {
                gm.StartFlashIcon();
            }
        }
        else
        {
            await SignInAnonymously();
        }
    }

    private void TurnOffLoadScreen()
    {
        loginScreen.SetActive(false);
    }

    public async Task SignInAnonymously()
    {
        Debug.Log("unity services state: " + UnityServices.State);
        string state = UnityServices.State.ToString();
        AuthenticationService.Instance.SignedIn += () =>
        {
            var playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in as: " + playerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
            ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = s.ToString();
            ConnectionErrorText.SetActive(true);
        };

        if (state.Equals("Initialized"))
        {
            await SignInAnonymouslyAsync();
        }
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            playerId.text = AuthenticationService.Instance.PlayerId;
            load.CheckForLoadData();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Retrying.. If problem persists Please check internet connection and restart the app.";
            ConnectionErrorText.SetActive(true);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Retrying.. If problem persists Please check internet connection and restart the app.";
            ConnectionErrorText.SetActive(true);
        }
    }
}
