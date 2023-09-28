using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region Singleton
    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Variables

    public Transform playerDisplayPos;
    public Transform customizationDisplayPoint;

    [Header("Next Scene")]
    public int sceneindex;

    [Header("UI Items")]
    public Button selectButton;
    public Image iconImage;
    public TextMeshProUGUI coinText;

    [Header("Private Properties")]
    CharacterDatabase characterDB;

    public int SelectedCharacter;
    public CharacterSkin skin;

    [Header("Menu Windows")]

    [Header("Customization Window")]
    public GameObject customizationWindow;
    public GameObject customizationCam;

    [Header("MainMenu Window")]
    public GameObject mainMenuWindow;
    public GameObject mainMenuCam;

    [Header("Settings Window")]
    public GameObject settingsWindow;

    [Header("Stores Window")]
    public GameObject storesWindow;

    [Header("Game Loading Window")]
    public GameObject gameLoadingWindow;
    public GameObject connectionLoader;

    [Header("Leaderboard Window")]
    public GameObject leaderboardWindow;

    [Header("Other")]
    public GameObject currencyHUD;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        characterDB = GameManager.instance.characterDatabase;

        //UseSkin(GameManager.instance.DisplaySkin());

        if(coinText != null)
            coinText.text = GameManager.instance.score.ToString();

        storesWindow = MenuManager.instance.storeWindow;

        //if(playerDisplayPos.childCount > 0)
        //{
        //    for (int i = 0; i < playerDisplayPos.childCount; i++)
        //    {
        //        Destroy(playerDisplayPos.GetChild(i).gameObject);
        //    }
        //}

        //Instantiate(GameManager.instance.DisplaySkin(), playerDisplayPos);
    }

    public void ConnectivityLoader(bool show)
    {
        connectionLoader.SetActive(show);
    }


    #region Customization

    public void OpenCustomizationWindow()
    {
        UpdateCharacter(SelectedCharacter);
        customizationCam.SetActive(true);
        mainMenuCam.SetActive(false);

        customizationWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void CloseCustomizationWindow()
    {
        mainMenuCam.SetActive(true);
        customizationCam.SetActive(false);

        mainMenuWindow.SetActive(true);
        customizationWindow.SetActive(false);
    }

    public void NextSkin()
    {
        SelectedCharacter++;

        if (SelectedCharacter >= characterDB.CharacterCount)
        {
            SelectedCharacter = 0;
        }

        UpdateCharacter(SelectedCharacter);
    }

    public void PrevSkin()
    {
        SelectedCharacter--;

        if (SelectedCharacter < 0)
        {
            SelectedCharacter = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(SelectedCharacter);
    }

    public void SelectSkin()
    {
        GameManager.instance.currentSkin = skin;
        UseSkin(GameManager.instance.currentSkin.skin);
    }

    public void UseSkin(GameObject newSkin)
    {
        if (playerDisplayPos.childCount > 0)
        {
            for (int i = 0; i < playerDisplayPos.childCount; i++)
            {
                Destroy(playerDisplayPos.GetChild(i).gameObject);
            }
        }

        Instantiate(newSkin, playerDisplayPos);
    }

    public void UpdateCharacter(int selectedCharacter)
    {
        Character character = characterDB.GetCharacter(selectedCharacter); // access characters index value from character database(Scriptableobject)
        selectButton.interactable = character.unlocked;
        iconImage.sprite = character.icon;
        skin = character.skin;
        GameObject slectedSkin = skin.skin;


        //Debug.Log("Show  ======"+ CharacterPOSInCharacterPnl.GetChild(0).gameObject);

        if (customizationDisplayPoint.childCount > 0) // if there is a character GAmeObject present, destroy it (so new gameobject can be shown there) 
        {
            for (int i = 0; i < customizationDisplayPoint.childCount; i++)
            {
                Destroy(customizationDisplayPoint.GetChild(i).gameObject);

            }
        }

        GameObject ShowingSkin = Instantiate(slectedSkin, customizationDisplayPoint);
    }

    #endregion


    #region MenuFunctions

    public void EnterMetaverse()
    {
        currencyHUD.SetActive(false);
        LoadLevel(sceneindex);
    }

    public virtual void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadSceneAsync(levelIndex);
    }

    public void OpenStoreWindow()
    {
        storesWindow.SetActive(true);
    }

    public void CloseStoreWindow()
    {
        storesWindow.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        currencyHUD.SetActive(false);
        settingsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        currencyHUD.SetActive(true);
        mainMenuWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }

    public void OpenLeaderboard()
    {
        LeaderBoardContentView.instance.RefreshLeaderboards();
        leaderboardWindow.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        leaderboardWindow.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
