using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using Unity.Services.Samples.VirtualShop;
using UnityEngine;

public class AddBalance : MonoBehaviour
{
    public string id;
    public int amount;

    static Task<GetBalancesResult> GetNewBalances()
    {
        var options = new GetBalancesOptions { ItemsPerFetch = 100 };
        return EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
    }

    public async Task SetNewBalance(string id, int amount)
    {
        await EconomyService.Instance.PlayerBalances.SetBalanceAsync(id, amount);
    }

    public async void AddNewBalance()
    {
        await SetNewBalance(id, amount);
        await EconomyManager.instance.RefreshCurrencyBalances();
    }
}
