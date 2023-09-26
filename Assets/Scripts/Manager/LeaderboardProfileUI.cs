using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardProfileUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI scoreText;

    public void SetData(string playerName, int rank, double score)
    {
        // Update the UI Text components with the data
        playerNameText.text = playerName.Split('#')[0]; // Get the playerName before '#'
        rankText.text = (rank + 1).ToString(); // Add 1 to make it 1-based rank
        scoreText.text = score.ToString();
    }
}
