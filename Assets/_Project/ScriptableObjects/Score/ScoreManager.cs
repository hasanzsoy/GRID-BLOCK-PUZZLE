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
        int earnedScore =
            blockCount * scoreSettings.scorePerBlock;

        currentScore += earnedScore;

        UpdateScoreText();

        Debug.Log(
            "Parça puanı: " + earnedScore +
            " | Toplam skor: " + currentScore
        );
    }

    public void AddLineClearScore(int clearedLines)
    {
        if (clearedLines <= 0)
        {
            return;
        }

        int bonusScore = 0;

        if (clearedLines == 1)
        {
            bonusScore =
                scoreSettings.oneLineBonus;
        }
        else if (clearedLines == 2)
        {
            bonusScore =
                scoreSettings.twoLineBonus;
        }
        else if (clearedLines == 3)
        {
            bonusScore =
                scoreSettings.threeLineBonus;
        }
        else
        {
            bonusScore =
                scoreSettings.fourOrMoreLineBonus;
        }

        currentScore += bonusScore;

        UpdateScoreText();

        Debug.Log(
            "Line bonus: +" + bonusScore +
            " | Temizlenen çizgi: " + clearedLines +
            " | Toplam skor: " + currentScore
        );
    }

    public void ResetScore()
    {
        currentScore = 0;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text =
            currentScore.ToString();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}