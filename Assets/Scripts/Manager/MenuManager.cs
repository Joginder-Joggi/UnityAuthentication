using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject logInPage;
    public GameObject SignUpPage;
    public GameObject LogOutPage;

    private void Start()
    {
        GoToLogInPage();
    }

    public void GoToSignUpPage()
    {
        logInPage.SetActive(false);
        SignUpPage.SetActive(true);
    }

    public void GoToLogInPage()
    {
        SignUpPage.SetActive(false);
        logInPage.SetActive(true);
    }
}
