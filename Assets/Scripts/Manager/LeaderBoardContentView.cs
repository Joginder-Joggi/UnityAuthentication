using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderBoardContentView : MonoBehaviour
{
    #region singleton

    public static LeaderBoardContentView instance;

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

    #region PublicVariables

    public GameObject leaderboardEntryPrefab;

    public Transform contentContainer;

    #endregion

    public async void RefreshLeaderboards()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("my_first_leaderboard");

        // Clear existing entries (if any)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate leaderboard entries for each result in scoresResponse
        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            var result = scoresResponse.Results[i];
            GameObject entry = Instantiate(leaderboardEntryPrefab, contentContainer);
            LeaderboardProfileUI entryUI = entry.GetComponent<LeaderboardProfileUI>();

            // Set the data for the entry
            entryUI.SetData(result.PlayerName, result.Rank, result.Score);
        }

        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}

public class LeaderboardPlayer
{
    public string playerName;
    public string playerScore;
    public string rank;
}
