using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text bestScoreText;
    public TMP_Text newHighScoreText;
    private int bestScore;
    private bool newHighScoreShownThisGame;
    private const string BestScoreKey = "BestScore";

    private void Start()
    {
        LoadBestScore();
        ResetForNewGame();
    }

    public void CheckScore(int currentScore)
    {
        if (currentScore <= bestScore)
        {
            return;
        }
        bestScore = currentScore;
        SaveBestScore();
        UpdateBestScoreText();
        if (!newHighScoreShownThisGame)
        {
            newHighScoreShownThisGame = true;
            ShowNewHighScore();
            Debug.Log("NEW HIGH SCORE! Yeni rekor: " + bestScore);
        }
    }

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt(BestScoreKey,0);
        UpdateBestScoreText();
        Debug.Log("Best Score yüklendi: " + bestScore);
    }
    private void SaveBestScore()
    {
        PlayerPrefs.SetInt(BestScoreKey,bestScore);
        PlayerPrefs.Save();
    }
    private void UpdateBestScoreText()
    {
        if (bestScoreText == null)
        {
            return;
        }

        bestScoreText.text = "BEST\n" + bestScore;
    }

    private void ShowNewHighScore()
    {
        if (newHighScoreText == null)
        {
            return;
        }
        newHighScoreText.gameObject.SetActive(true);
    }

    public void ResetForNewGame()
    {
        newHighScoreShownThisGame = false;
        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(false);
        }
    }

    public int GetBestScore()
    {
        return bestScore;
    }
    public bool HasNewHighScoreThisGame()
    {
        return newHighScoreShownThisGame;
    }
}