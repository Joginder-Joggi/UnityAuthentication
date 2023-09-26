using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Newtonsoft.Json;
using UnityEngine.SocialPlatforms.Impl;
using System;
using TMPro;

public class LeaderboardsManager : MonoBehaviour
{
    #region Singleton

    public static LeaderboardsManager instance;

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

    public TextMeshProUGUI text;

    public static string leaderboardId = new string("my_first_leaderboard");

    public string versionId;

    public int score;

    public async void AddScore()
    {
        var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);

        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(leaderboardId);

        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            Debug.Log("The score of Player " + scoresResponse.Results[i].PlayerName + " is " + scoresResponse.Results[i].Score);
        }

        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardId,
            new GetScoresOptions { Offset = 25, Limit = 50 }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerRange()
    {
        // Returns a total of 11 entries (the given player plus 5 on either side)
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
            leaderboardId,
            new GetPlayerRangeOptions { RangeLimit = rangeLimit }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetScoresByPlayerIds()
    {
        var otherPlayerIds = new List<string> { "abc123", "abc456" };
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresByPlayerIdsAsync(leaderboardId, otherPlayerIds);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetScoresByTier()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresByTierAsync(leaderboardId, "silver");
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScoresByTier()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresByTierAsync(
            leaderboardId,
            "silver",
            new GetScoresByTierOptions { Offset = 25, Limit = 50 }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetLeaderboardVersions()
    {
        var versionsResponse = await LeaderboardsService.Instance
            .GetVersionsAsync(leaderboardId);

        // Get the ID of the most recently archived Leaderboard version
        var versionId = versionsResponse.Results[0].Id;

        Debug.Log(JsonConvert.SerializeObject(versionsResponse.Results));
    }

    public async void GetPlayerVersionScore()
    {
        var versionId = "";
        var scoreResponse = await LeaderboardsService.Instance
            .GetVersionPlayerScoreAsync(leaderboardId, versionId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse.Score));
    }

    public async void GetVersionScores()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetVersionScoresAsync(leaderboardId, versionId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetVersionPlayerRange()
    {
        var versionIds = versionId;
        // Returns a total of 11 entries (the given player plus 5 on either side)
        var rangeLimit = 5;
        var response = await LeaderboardsService.Instance.GetVersionPlayerRangeAsync(
            leaderboardId,
            versionIds,
            new GetVersionPlayerRangeOptions { RangeLimit = rangeLimit }
        );
        Debug.Log(JsonConvert.SerializeObject(response));
    }

    public async void GetScoresByPlayerIdsVersion()
    {
        var otherPlayerIds = new List<string> { "abc123", "abc456" };
        var scoresResponse = await LeaderboardsService.Instance
            .GetVersionScoresByPlayerIdsAsync(leaderboardId, versionId, otherPlayerIds);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetVersionScoresByTier()
    {
        var versionId = "";
        var scoresResponse = await LeaderboardsService.Instance
            .GetVersionScoresByTierAsync(leaderboardId, versionId, "silver");
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedVersionScoresByTier()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetVersionScoresByTierAsync(
            leaderboardId,
            versionId,
            "silver",
            new GetVersionScoresByTierOptions { Offset = 25, Limit = 50 }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}
