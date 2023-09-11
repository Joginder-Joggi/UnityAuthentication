using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class CloudSaveSystem : MonoBehaviour
{
    public static CloudSaveSystem instance;

    public TMP_InputField CoinInput;
    public TMP_Text ShowCoinText;

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

    public async void Start()
    {
        await UnityServices.InitializeAsync();

        LoadDataOnStart();
    }
   

    public async void SaveCoinsBtnPress()
    {
        int _coinInput = int.Parse(CoinInput.text);         
        await SaveDataAsync(_coinInput);
    }
    public async Task SaveDataAsync(int Coins)
    {
        try
        {
            var data = new Dictionary<string, object> { { "CoinsData", Coins} };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);

        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, show a message, etc.).
            Console.WriteLine($"An error occurred while saving data: {ex.Message}");
        }
    }


    private async void LoadDataOnStart()
    {
        Dictionary<string, string> CoinData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "CoinsData" });

        if (CoinData.ContainsKey("CoinsData"))
        {
            string coinsDataString = CoinData["CoinsData"];

            if (int.TryParse(coinsDataString, out int coinsDataInt))
            {
                ShowCoinText.text = coinsDataInt.ToString(); // Display the integer value in the text box
            }
            else
            {
                print("Invalid integer format in CoinsData key");
            }
        }
        else
        {
           // coin key will be made
            var data = new Dictionary<string, object> { { "CoinsData", 0 } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Dictionary<string, string> coinData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "CoinsData" });
            if (coinData.ContainsKey("CoinsData"))
            {
                string CcoinsDataString = coinData["CoinsData"];

                if (int.TryParse(CcoinsDataString, out int coinsDataInt))
                {
                    ShowCoinText.text = coinsDataInt.ToString(); // Display the integer value in the text box
                }
                else
                {
                    print("Invalid integer format in CoinsData key");
                }
            }
        }
    }


    public async void LoadData()
    {
        Dictionary<string, string> CCoinData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "CoinsData" });

        if (CCoinData.ContainsKey("CoinsData"))
        {
            string cooinsDataString = CCoinData["CoinsData"];

            if (int.TryParse(cooinsDataString, out int coinsDataInt))
            {
                ShowCoinText.text = coinsDataInt.ToString(); // Display the integer value in the text box
            }
            else
            {
                print("Invalid integer format in CoinsData key");
            }
        }
        else
        {
            print("Key not present");
        }
    }

}
