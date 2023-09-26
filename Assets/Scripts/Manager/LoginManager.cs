using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using DG.Tweening;
using Unity.Services.Samples.VirtualShop;

public class LoginManager : MonoBehaviour
{
    #region Variables

    [Header("Profile")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI titleText;

    IAuthenticationService authenticationService;

    [Header("SignIn")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI messageText;
    public Toggle autoSignIn;

    [Header("SignIn Keys(PlayerPrefs)")]
    public string usernameKey = "Username";
    public string passwordKey = "Password";
    public string autoSignInKey = "Auto SignIn";

    [Header("SignUp")]
    public TMP_InputField _usernameField;
    public TMP_InputField _passwordField;
    public TMP_InputField _confirmPasswordField;
    public Toggle termsAndCondition;
    public Button signUpButton;

    #endregion

    #region General Code

    public async void Start()
    {
        await UnityServices.InitializeAsync();
        authenticationService = AuthenticationService.Instance;

        //Initializing Authentication Callbacks
        authenticationService.Expired += OnSessionExpired;
        authenticationService.SignedIn += OnSignedIn;
        authenticationService.SignedOut += OnSignedOut;
        authenticationService.SignInFailed += OnSignInFailed;

        MenuManager.instance.signedIn = authenticationService.IsSignedIn;

        signUpButton.interactable = termsAndCondition.isOn;

        if (PlayerPrefs.HasKey(autoSignInKey))
        {
            int i = PlayerPrefs.GetInt(autoSignInKey);

            if(i == 1)
            {
                autoSignIn.isOn = true;
            }
            else
            {
                autoSignIn.isOn = false;
            }
        }

        if(autoSignIn.isOn && PlayerPrefs.HasKey(usernameKey) && PlayerPrefs.HasKey(passwordKey))
        {
            usernameField.text = PlayerPrefs.GetString(usernameKey);
            passwordField.text = PlayerPrefs.GetString(passwordKey);
            SignInAsync();
        }


        StartCoroutine(ShowMessage(messageText, "Welcome to the Tarality", Color.blue));
    }

    public void AgreeToTheTerms(bool agree)
    {
        signUpButton.interactable = agree;
    }


    public void SetProfile(string name)
    {
        usernameText.text = name;
        titleText.text = "Rising Champ";
    }

    public IEnumerator ShowMessage(TextMeshProUGUI textObject, string message, Color color)
    {
        textObject.color = color;
        textObject.text = message;

        yield return new WaitForSeconds(1.5f);

        textObject.color = Color.white;
        if (authenticationService.IsSignedIn)
        {
            textObject.text = "Signed In As " + usernameText.text;
        }
        else
        {
            textObject.text = "Sign In Required !";
        }
    }

    #endregion

    #region SignIn/Out Region

    public async void SignInAsync()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (autoSignIn.isOn)
        {
            PlayerPrefs.SetString(usernameKey, username);
            PlayerPrefs.SetString(passwordKey, password);
            PlayerPrefs.SetInt(autoSignInKey, 1);
        }
        else
        {
            PlayerPrefs.DeleteKey(usernameKey);
            PlayerPrefs.DeleteKey(passwordKey);
            PlayerPrefs.SetInt(autoSignInKey, 0);
        }

        MainMenuManager.instance.ConnectivityLoader(true);

        if (username != "" &&  password != "")
        {
            await SignInWithUsernamePasswordAsync(username, password);
            //SceneManager.LoadScene(1);
        }
        else
        {
            StartCoroutine(ShowMessage(messageText, "Please enter credantials", Color.red));
        }


        MainMenuManager.instance.ConnectivityLoader(false);
    }

    public void SignOut()
    {
        authenticationService.SignOut(false);
        SetProfile("");
        StartCoroutine(ShowMessage(messageText, "You Successfuly Signed Out", Color.yellow));
        MenuManager.instance.GoToSignInPage();
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {


        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            SetProfile(username);
            StartCoroutine(ShowMessage(messageText, "Sign In Successfull", Color.green));
            Debug.Log("SignIn is successful.");
            MenuManager.instance.ShowProfileWindow();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
            Debug.LogException(ex);
        }


    }

    #endregion

    #region Guest SignIn Region

    public async void SingInAsGuest()
    {
        MainMenuManager.instance.ConnectivityLoader(true);

        await SignInAnonymouslyAsync();

        MainMenuManager.instance.ConnectivityLoader(false);
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            usernameText.text = "Guest";
            Debug.Log("Sign in anonymously succeeded!");
            StartCoroutine(ShowMessage(messageText, "Sign In As Guest Successfull", Color.red));
            MenuManager.instance.isGuest = true;
            MenuManager.instance.signUpButton_Profile.SetActive(MenuManager.instance.isGuest);
            MenuManager.instance.ShowProfileWindow();

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
        }
    }

    #endregion

    #region SignUpRegion

    public async void SignUpAsync()
    {
        string username = _usernameField.text;
        string password = _passwordField.text;
        string confirmPassword = _confirmPasswordField.text;
        //string email = _emailField.text;

        // Implement user registration logic here.
        // You can use Unity's Authentication service or your custom registration system.
        // For Unity Authentication service, refer to Unity's documentation.

        MainMenuManager.instance.ConnectivityLoader(true);

        if (password != "" && password == confirmPassword)
        {
            if(MenuManager.instance.signedIn && MenuManager.instance.isGuest)
            {
                await AddUsernamePasswordAsync(username, password);
            }
            else
            {
                await SignUpWithUsernamePasswordAsync(username, password);
            }

            SetProfile(username);
            //SceneManager.LoadScene(1);
        }
        else if(password == "" ||  password != confirmPassword)
        {
            StartCoroutine(ShowMessage(messageText, "Please check entered password", Color.red));
        }
        else if(password == "" && username == "")
        {
            StartCoroutine(ShowMessage(messageText, "Please fill required credantials", Color.red));
        }


        MainMenuManager.instance.ConnectivityLoader(false);
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            StartCoroutine(ShowMessage(messageText, "Sign Up Successfull", Color.green));
            Debug.Log("SignUp is successful.");
            MenuManager.instance.ShowProfileWindow();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            StartCoroutine(ShowMessage(messageText, ex.Message, Color.red));
            Debug.LogException(ex);
        }
    }

    #endregion

    #region Guest To User SignUp
    async Task AddUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
            Debug.Log("Username and password added.");
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

    #endregion

    #region SessionCallBacks

    // Callback when the session expires
    private void OnSessionExpired()
    {
        Debug.Log("Session has expired.");
        MenuManager.instance.signedIn = true;
        MenuManager.instance.PopSessionTimeOutWindow();
        if (MenuManager.instance.isGuest) MenuManager.instance.isGuest = false;
    }

    // Callback when a sign-in attempt has completed successfully
    private void OnSignedIn()
    {
        Debug.Log("Player has signed in.");
        MenuManager.instance.signedIn = true;
        VirtualShopSceneManager.Instance.InitializeStoreAsync();
    }

    // Callback when a sign-out attempt has completed successfully
    private void OnSignedOut()
    {
        Debug.Log("Player has signed out.");
        MenuManager.instance.signedIn = true;
        if (MenuManager.instance.isGuest) MenuManager.instance.isGuest = false;
    }

    // Callback when a sign-in attempt has failed
    private void OnSignInFailed(RequestFailedException exception)
    {
        Debug.LogError("Sign-in attempt failed: " + exception.Message);
        if (MenuManager.instance.signedIn) MenuManager.instance.signedIn = false;
        if (MenuManager.instance.isGuest) MenuManager.instance.isGuest = false;
    }

    #endregion
}
