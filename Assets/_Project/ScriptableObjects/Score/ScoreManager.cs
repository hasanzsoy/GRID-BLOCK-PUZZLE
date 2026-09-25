using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public ScoreSettingsSO scoreSettings;
    [Header("UI")]
    public TMP_Text scoreText;
    private int currentScore;
    private HighScoreManager highScoreManager;
    [Header("Score Animation")]
    [SerializeField] private float scorePunchScale = 0.20f;
    [SerializeField] private float scorePunchDuration = 0.20f;

    private void Awake()
    {
        highScoreManager = FindFirstObjectByType<HighScoreManager>();
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddPieceScore(int blockCount)
    {
        int earnedScore = blockCount * scoreSettings.scorePerBlock;
        currentScore += earnedScore;
        UpdateScoreText();
        PlayScorePunch();
        Debug.Log("Parça puanı: " + earnedScore +" | Toplam skor: " + currentScore);
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
            bonusScore = scoreSettings.oneLineBonus;
        }
        else if (clearedLines == 2)
        {
            bonusScore = scoreSettings.twoLineBonus;
        }
        else if (clearedLines == 3)
        {
            bonusScore = scoreSettings.threeLineBonus;
        }
        else
        {
            bonusScore = scoreSettings.fourOrMoreLineBonus;
        }
        currentScore += bonusScore;
        UpdateScoreText();
        PlayScorePunch();
        Debug.Log("Line bonus: +" +bonusScore +" | Temizlenen çizgi: " + clearedLines +" | Toplam skor: " +currentScore);
    }
    public void AddComboScore(int comboCount)
    {
        if (comboCount <= 1)
        {
            return;
        }
        int comboBonus = (comboCount - 1) * scoreSettings.comboBonusPerLevel;
        currentScore += comboBonus;
        UpdateScoreText();
        Debug.Log("Combo bonus: +" + comboBonus +" | Combo: " + comboCount +" | Toplam skor: " + currentScore);
    }
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
        if (highScoreManager != null)
        {
            highScoreManager.CheckScore(currentScore);
        }
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }
    private void PlayScorePunch()
    {
        if (scoreText == null)
        {
            return;
        }
        scoreText.transform.DOKill();
        scoreText.transform.localScale = Vector3.one;
        scoreText.transform.DOPunchScale(Vector3.one * scorePunchScale,scorePunchDuration,5,0.5f);
    }
}