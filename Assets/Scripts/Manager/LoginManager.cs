using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    [Header("LogIn")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI errorText;

    [Header("SignUp")]
    public TMP_InputField _usernameField;
    public TMP_InputField _passwordField;
    public TMP_InputField _confirmPasswordField;
    public TextMeshProUGUI _errorText;

    public async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void LoginAsync()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if(username != "" &&  password != "")
        {
            await SignInWithUsernamePasswordAsync(username, password);
            SceneManager.LoadScene(1);
        }
        else
        {
            errorText.text = "Please Enter Credantials";
        }
    }    

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            errorText.text = "Sign In Sccessful";
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            errorText.text = ex.Message;
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            errorText.text = ex.Message;
            Debug.LogException(ex);
        }
    }

    public async void SignUpAsync()
    {
        string username = _usernameField.text;
        string password = _passwordField.text;
        string confirmPassword = _confirmPasswordField.text;
        //string email = _emailField.text;

        // Implement user registration logic here.
        // You can use Unity's Authentication service or your custom registration system.
        // For Unity Authentication service, refer to Unity's documentation.
        
        if(password != "" && password == confirmPassword)
        {
            await SignUpWithUsernamePasswordAsync(username, password);
            SceneManager.LoadScene(1);
        }
        else
        {
            errorText.text = "Please check entered password";
        }
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            errorText.text = "Sign Up Sccessful";
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            errorText.text = ex.Message;
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            errorText.text = ex.Message;
            Debug.LogException(ex);
        }
    }

    private bool RegisterUser(string username, string password, string email)
    {
        // Implement user registration logic here.
        // You can use Unity's Authentication service or your custom registration system.
        // For Unity Authentication service, refer to Unity's documentation.
        return false; // Change this to return true if registration succeeds.
    }

    private async Task<bool> AuthenticateUserAsync(string username, string password)
    {
        // Implement user authentication logic here.
        // You can use Unity's Authentication service or your custom authentication system.
        // For Unity Authentication service, refer to Unity's documentation.

        await SignInWithUsernamePasswordAsync(username, password);

        return false; // Change this to return true if authentication succeeds.
    }
}
