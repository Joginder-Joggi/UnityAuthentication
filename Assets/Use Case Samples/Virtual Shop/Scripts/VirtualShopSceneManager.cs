﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using GAMEDEV.Save;
using UnityEngine.UI;

namespace Unity.Services.Samples.VirtualShop
{
    public class VirtualShopSceneManager : MonoBehaviour
    {
        public static VirtualShopSceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        const int k_EconomyPurchaseCostsNotMetStatusCode = 10504;

        public VirtualShopSampleView virtualShopSampleView;

        //async void Start()
        //{
        //    //await InitializeShopAsync();
        //}

        public async void InitializeStoreAsync()
        {
            await InitializeShopAsync();
        }

        public async Task InitializeShopAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    //await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    //if (this == null) return;
                    MenuManager.instance.GoToSignInPage();
                    return;
                }

                Debug.Log($"Player id:{AuthenticationService.Instance.PlayerId}");

                await EconomyManager.instance.RefreshEconomyConfiguration();
                if (this == null) return;

                EconomyManager.instance.InitializeVirtualPurchaseLookup();

                // Note: We want these methods to use the most up to date configuration data, so we will wait to
                // call them until the previous two methods (which update the configuration data) have completed.
                await Task.WhenAll(AddressablesManager.instance.PreloadAllEconomySprites(),
                    RemoteConfigManager.instance.FetchConfigs(),
                    EconomyManager.instance.RefreshCurrencyBalances());
                if (this == null) return;

                // Read all badge addressables
                // Note: must be done after Remote Config values have been read (above).
                await AddressablesManager.instance.PreloadAllShopBadgeSprites(
                    RemoteConfigManager.instance.virtualShopConfig.categories);

                // Initialize all shops.
                // Note: must be done after all other initialization has completed (above).
                VirtualShopManager.instance.Initialize();

                virtualShopSampleView.Initialize(VirtualShopManager.instance.virtualShopCategories);

                var firstCategoryId = RemoteConfigManager.instance.virtualShopConfig.categories[0].id;
                if (!VirtualShopManager.instance.virtualShopCategories.TryGetValue(
                        firstCategoryId, out var firstCategory))
                {
                    throw new KeyNotFoundException($"Unable to find shop category {firstCategoryId}.");
                }

                virtualShopSampleView.ShowCategory(firstCategory);

                Debug.Log("Initialization and sign in complete.");

                EnablePurchases();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void EnablePurchases()
        {
            virtualShopSampleView.SetInteractable();
        }

        public void OnCategoryButtonClicked(string categoryId)
        {
            var virtualShopCategory = VirtualShopManager.instance.virtualShopCategories[categoryId];
            virtualShopSampleView.ShowCategory(virtualShopCategory);
        }

        public async Task OnPurchaseClicked(VirtualShopItem virtualShopItem, Button button)
        {
            try
            {
                var result = await EconomyManager.instance.MakeVirtualPurchaseAsync(virtualShopItem.id);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
                if (this == null) return;

                ShowRewardPopup(result.Rewards, virtualShopItem, button);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void OnGainCurrencyDebugButtonClicked()
        {
            try
            {
                await EconomyManager.instance.GrantDebugCurrency("GEM", 30);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void SaveEarnedCurrency(Currency[] currencies)
        {
            foreach (Currency currency in currencies)
            {
                try
                {
                    await EconomyManager.instance.GrantDebugCurrency(currency.id, currency.amount);
                    if (this == null) return;

                    await EconomyManager.instance.RefreshCurrencyBalances();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        void ShowRewardPopup(Rewards rewards, VirtualShopItem virtualShopItem, Button button)
        {
            var addressablesManager = AddressablesManager.instance;

            var rewardDetails = new List<RewardDetail>();
            foreach (var inventoryReward in rewards.Inventory)
            {
                rewardDetails.Add(new RewardDetail
                {
                    id = inventoryReward.Id,
                    quantity = inventoryReward.Amount,
                    sprite = addressablesManager.preloadedSpritesByEconomyId[inventoryReward.Id]
                });

                Debug.Log("The Name Is Andi mandi " + inventoryReward.Id + " & " + virtualShopItem.id);
                SaveItemData(virtualShopItem.id, 1);
                CloudSaveSystem.Interaction(button, false);
            }

            foreach (var currencyReward in rewards.Currency)
            {
                rewardDetails.Add(new RewardDetail
                {
                    id = currencyReward.Id,
                    quantity = currencyReward.Amount,
                    sprite = addressablesManager.preloadedSpritesByEconomyId[currencyReward.Id]
                });
            }

            virtualShopSampleView.ShowRewardPopup(rewardDetails);
        }

        async void SaveItemData(string id, int value)
        {
            await CloudSaveSystem.SaveDataAsync(id, 1);
        }
    }
}
