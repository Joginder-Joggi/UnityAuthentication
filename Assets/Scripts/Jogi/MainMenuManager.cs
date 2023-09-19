using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    public Transform playerDisplayPos;
    public Transform customizationDisplayPoint;

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

    // Start is called before the first frame update
    void Start()
    {
        characterDB = GameManager.instance.characterDatabase;

        UseSkin(GameManager.instance.DisplaySkin());

        coinText.text = GameManager.instance.coins.ToString();

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectivityLoader(bool show)
    {
        connectionLoader.SetActive(show);
    }

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

    public void OpenStoreWindow()
    {
        storesWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void CloseStoreWindow()
    {
        mainMenuWindow.SetActive(true);
        storesWindow.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        mainMenuWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
