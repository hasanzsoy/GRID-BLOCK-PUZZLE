using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public ScoreSettingsSO scoreSettings;

    [Header("UI")]
    public TMP_Text scoreText;

    private int currentScore;

    private void Start()
    {
        ResetScore();
    }

    public void AddPieceScore(int blockCount)
    {
        int earnedScore = blockCount * scoreSettings.scorePerBlock;

        currentScore += earnedScore;

        UpdateScoreText();

        Debug.Log("Kazanılan puan: " + earnedScore +" | Toplam skor: " + currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}