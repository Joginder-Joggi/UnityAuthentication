using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GAMEDEV.Save
{
    public static class CloudSaveSystem
    {
        public static async Task SaveDataAsync(string id, int value)
        {
            try
            {
                var data = new Dictionary<string, object> { { id, value } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);

            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (log, show a message, etc.).
                Console.WriteLine($"An error occurred while saving data: {ex.Message}");
            }
        }

        public static async Task<int> LoadDataAsync(string id)
        {
            Dictionary<string, string> Data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { id });

            int _dataInt;

            if (Data.ContainsKey(id))
            {
                string dataString = Data[id];

                if (int.TryParse(dataString, out int dataInt))
                {
                    return _dataInt = dataInt;
                }
            }

            return _dataInt = 0;
        }

        public static void Interaction(Button button, bool value)
        {
            if (button != null)
            {
                button.interactable = value;
            }
        }

    }
}
