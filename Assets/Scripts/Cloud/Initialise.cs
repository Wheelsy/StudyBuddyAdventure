using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;
using GooglePlayGames;
using UnityEngine.Android;

public class Initialise : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerId;
    [SerializeField]
    private GameObject connectionErrorText;
    [SerializeField]
    private GameObject loginScreen;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
        InitializePlayGamesLogin();
    }

    void InitializePlayGamesLogin()
    {
        PlayGamesPlatform.Activate();
    }

    // Call when login button is pressed
    public void LoginGooglePlayGames()
    {       
        Social.localUser.Authenticate(OnGooglePlayGamesLogin);
    }

    //Check login success
    async void OnGooglePlayGamesLogin(bool success)
    {
        if (success)
        {
            // Call Unity Authentication SDK to sign in or link with Google.
            string idToken = PlayGamesPlatform.Instance.GetUserId();
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
            //if sign in successful check for load data and assign player id to in game ui.
            //Turn login screen off.
            loginScreen.SetActive(false);
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
