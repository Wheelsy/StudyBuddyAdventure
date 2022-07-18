using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Android;

public class Initialise : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerId;
    [SerializeField]
    private GameObject connectionErrorText;
    [SerializeField]
    private GameObject loginScreen;

    private CloudLoad load;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    async void Start()
    {
        load = gameObject.GetComponent<CloudLoad>();
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
            connectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Please check internet connection and restart the app.";
            connectionErrorText.SetActive(true);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            connectionErrorText.GetComponent<TextMeshProUGUI>().text = "Error: Unable to sign in. Please check internet connection and restart the app.";
            connectionErrorText.SetActive(true);
        }
    }
}
