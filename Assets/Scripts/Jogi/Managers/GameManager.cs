using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Core;
using Unity.Services.CloudSave;
//using UnityEditor.Animations;
using UnityEngine;
using Unity.Services.Samples.VirtualShop;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
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

    #region Public Properties

    public CharacterDatabase characterDatabase;

    public CharacterSkin currentSkin = null;
    public CharacterSkin defaultSkin;
    public RuntimeAnimatorController playerController;
    public ControlMode controlMode;

    public int score;
    public int highscore;

    [Header("Currencies")]
    public Currency[] myCurrencies;

    [Header("Player Pref Keys")]
    public string coinKey;
    public string skinIdKey;

    #endregion

    private void Start()
    {
        //if (PlayerPrefs.HasKey(coinKey))
        //{
        //    coins = PlayerPrefs.GetInt(coinKey);
        //}

        if(Application.platform == RuntimePlatform.Android && controlMode != ControlMode.TOUCH)
        {
            controlMode = ControlMode.TOUCH;
        }
    }

    public GameObject DisplaySkin()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            int id = PlayerPrefs.GetInt("PlayerName");
            currentSkin = characterDatabase.GetCharacter(id).skin;
        }
        else
        {
            currentSkin = defaultSkin;
        }

        GameObject skin = currentSkin.skin;

        return skin;
    }

    internal void ShowOffer()
    {
        Debug.Log("Not Enough Coins.. Earn or Buy some");
    }

    public async Task SetHighscoreAsync()
    {
        int highscore = await LeaderBoardContentView.instance.GetPlayersScore();
        this.highscore = highscore;
    }

    public void UpdateScores(Currency[] currencies, int score)
    {
        AddCoins(currencies);

        if(score > highscore)
        {
            highscore = score;
            LeaderboardsManager.instance.AddScore(score);
        }
    }

    public void AddCoins(Currency[] collectedCurrencies)
    {
        //foreach (var collectedCurrency in collectedCurrencies)
        //{
        //    foreach (var myCurrency in myCurrencies)
        //    {
        //        if(string.Equals(collectedCurrency.id, myCurrency.id))
        //        {
        //            Debug.Log(myCurrency.amount);
        //            Debug.Log(collectedCurrency.amount);
        //            myCurrency.amount += collectedCurrency.amount;
        //        }
        //    }
        //}


        VirtualShopSceneManager.Instance.SaveEarnedCurrency(collectedCurrencies);
        //PlayerPrefs.SetInt(coinKey, coins);
    }
}

[System.Serializable]
public class Currency
{
    public string id;
    public int amount;

    public void SetBalance(int balance)
    {
        amount = balance;
    }
}

public enum ControlMode
{
    TOUCH,
    KEYBOARD
}

public enum CurrencyType
{
    COIN,
    GEM
}
