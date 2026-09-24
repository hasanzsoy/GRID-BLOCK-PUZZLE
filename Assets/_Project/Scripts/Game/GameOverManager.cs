using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    private ScoreManager scoreManager;
    private ComboManager comboManager;
    private BoardManager boardManager;
    private PieceTrayManager pieceTrayManager;
    private HighScoreManager highScoreManager;

    private bool isGameOver;

    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        comboManager = FindFirstObjectByType<ComboManager>();
        boardManager = FindFirstObjectByType<BoardManager>();
        pieceTrayManager = FindFirstObjectByType<PieceTrayManager>();
        highScoreManager = FindFirstObjectByType<HighScoreManager>();
    }

    private void Start()
    {
        isGameOver = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (isGameOver)
        {
            return;
        }
        isGameOver = true;
        if (gameOverPanel == null)
        {
            return;
        }
        int finalScore = scoreManager.GetCurrentScore();
        if (finalScoreText != null)
        {
            finalScoreText.text ="SKOR\n" + finalScore;
        }
        gameOverPanel.SetActive(true);
        Debug.Log("GAME OVER | Final Score: " + finalScore);
    }
    public void RestartGame()
    {
        Debug.Log("Oyun yeniden başlatılıyor...");
        boardManager.ResetBoard();
        scoreManager.ResetScore();
        comboManager.ResetCombo();
        highScoreManager.ResetForNewGame();
        pieceTrayManager.ResetTray();
        isGameOver = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        Debug.Log("Yeni oyun başladı.");
    }
}