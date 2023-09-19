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

    public int coins;

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

    public void AddCoins(Currency[] collectedCurrencies)
    {
        foreach (var collectedCurrency in collectedCurrencies)
        {
            foreach (var myCurrency in myCurrencies)
            {
                if(string.Equals(collectedCurrency.id, myCurrency.id))
                {
                    myCurrency.amount += collectedCurrency.amount;
                }
            }
        }


        VirtualShopSceneManager.Instance.SaveEarnedCurrency(myCurrencies);
        //PlayerPrefs.SetInt(coinKey, coins);
    }

    public void DeductCoins(int amount)
    {
        coins -= amount;
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

public enum CurrencyType
{
    COIN,
    GEM
}

public enum ControlMode
{
    TOUCH,
    KEYBOARD
}
