using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using System;

public class Initialise : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerId;
    [SerializeField]
    private GameObject connectionErrorText;
    [SerializeField]
    private GameObject loginScreen;

    private CloudLoad load;

    public GameObject ConnectionErrorText { get => connectionErrorText; set => connectionErrorText = value; }

    internal async Task Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymously();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void Start()
    {
        load = gameObject.GetComponent<CloudLoad>();
    }

    private async Task SignInAnonymously()
    {
        Debug.Log("unity services state: " + UnityServices.State);
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
            return;
        };

        await SignInAnonymouslyAsync();
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
            ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Please check internet connection and restart the app.";
            ConnectionErrorText.SetActive(true);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            ConnectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Please check internet connection and restart the app.";
            ConnectionErrorText.SetActive(true);
        }
    }
}
