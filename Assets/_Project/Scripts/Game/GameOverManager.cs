using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    private ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel == null)
        {
            return;
        }

        int finalScore = scoreManager.GetCurrentScore();

        if (finalScoreText != null)
        {
            finalScoreText.text = "SKOR\n" + finalScore;
        }

        gameOverPanel.SetActive(true);

        Debug.Log("GAME OVER | Final Score: " + finalScore);
    }
}