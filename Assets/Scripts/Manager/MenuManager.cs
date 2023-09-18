using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Singleton

    public static MenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Variables

    public GameObject signInPage;
    public GameObject SignUpPage;
    public GameObject profileWindow;
    public GameObject sessionTimeoutWindow;
    public GameObject authenticationWindow;
    public GameObject guestUserConfirmationScreen;
    public GameObject terms;
    public GameObject signUpButton_Profile;
    public GameObject connectivityLoader;

    public bool signedIn = false;
    public bool isGuest = true;

    #endregion

    #region Methods

    private void Start()
    {
        if (!signedIn)
        {
            GotoAccountsWindow();
            GoToSignInPage();
        }
        else
        {
            ShowProfileWindow();
        }
    }

    public void GoToSignUpPage()
    {
        EnableDisableWindows(profileWindow, signInPage, sessionTimeoutWindow, SignUpPage);
    }

    public void GoToSignInPage()
    {
        EnableDisableWindows(profileWindow, SignUpPage, sessionTimeoutWindow, signInPage);
    }

    public void ShowProfileWindow()
    {
        EnableDisableWindows(signInPage, sessionTimeoutWindow, SignUpPage, profileWindow);
    }

    public void PopSessionTimeOutWindow()
    {
        EnableDisableWindows(profileWindow, signInPage, SignUpPage, sessionTimeoutWindow);
    }


    public void GotoHomeScreen()
    {
        authenticationWindow.SetActive(false);
    }

    public void GotoAccountsWindow()
    {
        authenticationWindow.SetActive(true);
    }

    public void EnableDisableWindows(GameObject windowToDisableA, GameObject windowToDisableB, GameObject windowToDisableC, GameObject windowToEnable)
    {
        if(windowToDisableA.activeInHierarchy) windowToDisableA.SetActive(false);
        if (windowToDisableB.activeInHierarchy) windowToDisableB.SetActive(false);
        if (windowToDisableC.activeInHierarchy) windowToDisableC.SetActive(false);
        if (!windowToDisableA.activeInHierarchy) windowToEnable.SetActive(true);
    }

    public void GuestUserConfirmationScreen(bool show)
    {
        if (signedIn)
        {
            ShowProfileWindow();
        }
        else
        {
            guestUserConfirmationScreen.SetActive(show);
        }
    }

    public void ShowTerms(bool show)
    {
        terms.SetActive(show);
    }

    public void ConnectivityLoader(bool show)
    {
        connectivityLoader.SetActive(show);
    }

    #endregion
}
