using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardProfileUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI scoreText;

    public Image crown;

    public Sprite goldCrown;
    public Sprite silverCrown;
    public Sprite bronzeCrown;

    public void SetData(string playerName, int rank, double score)
    {
        if(rank <= 2)
        {
            switch (rank)
            {
                case 0:

                    crown.sprite = goldCrown;
                    break;

                case 1:

                    crown.sprite = silverCrown;
                    break;

                case 2:

                    crown.sprite = bronzeCrown;
                    break;
            }
        }
        else
        {
            crown.gameObject.SetActive(false);
        }

        // Update the UI Text components with the data
        playerNameText.text = playerName.Split('#')[0]; // Get the playerName before '#'
        rankText.text = (rank + 1).ToString(); // Add 1 to make it 1-based rank
        scoreText.text = score.ToString();
    }
}
