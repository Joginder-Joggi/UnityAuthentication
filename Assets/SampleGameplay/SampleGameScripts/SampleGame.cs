using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SampleGame : MonoBehaviour
{
    public int sceneIndex;

    public TMP_InputField coinV;
    public TMP_InputField gemV;
    public TMP_InputField scoreV;

    public TextMeshProUGUI coinField;
    public TextMeshProUGUI gemfield;
    public TextMeshProUGUI scoreField;

    public Currency[] collectedCurrency;

    int score;

    public void UpdateScores()
    {
        GameManager.instance.UpdateScores(collectedCurrency, score);
    }

    public void EarnCoin()
    {
        collectedCurrency[0].amount += int.Parse(coinV.text);
        coinField.text = collectedCurrency[0].amount.ToString();
    }

    public void EarnGem()
    {
        collectedCurrency[1].amount += int.Parse(gemV.text);
        gemfield.text = collectedCurrency[1].amount.ToString();
    }

    public void EarnScores()
    {
        score += int.Parse(scoreV.text);
        scoreField.text = score.ToString();
    }

    public void GoToHome()
    {
        UpdateScores();
        MainMenuManager.instance.currencyHUD.SetActive(true);
        MainMenuManager.instance.LoadLevel(sceneIndex);
    }
}
