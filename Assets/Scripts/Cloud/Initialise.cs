using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using TMPro;
using GooglePlayGames;

public class Initialise : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerId;
    [SerializeField]
    private GameObject connectionErrorText;
    [SerializeField]
    private GameObject loginScreen;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
    }

    private void Start()
    {
        InitializePlayGamesLogin();
        SetupEvents();
    }

    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    void InitializePlayGamesLogin()
    {      
        PlayGamesPlatform.Activate();
    }

    public void LoginGooglePlayGames()
    {
        Social.localUser.Authenticate(OnGooglePlayGamesLogin);
    }

    async void OnGooglePlayGamesLogin(bool success)
    {
        if (success)
        {
            // Call Unity Authentication SDK to sign in or link with Google.
            string idToken = PlayGamesPlatform.Instance.GetUserId();
            Debug.Log("Player id: " + idToken);
            await SignInWithGoogleAsync(idToken);
        }
        else
        {
            Debug.Log("Unsuccessful login");
        }
    }

    async Task SignInWithGoogleAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
            Debug.Log("SignIn is successful.");
            //if sign in successful check for load data and assign player id to in game ui
            loginScreen.SetActive(false);
            gameObject.GetComponent<CloudLoad>().CheckForLoadData();
            playerId.text = AuthenticationService.Instance.PlayerId.ToString();
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
    }
}
