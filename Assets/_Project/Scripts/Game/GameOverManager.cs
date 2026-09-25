using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text bestScoreText;
    public TMP_Text newRecordText;
    private ScoreManager scoreManager;
    private ComboManager comboManager;
    private BoardManager boardManager;
    private PieceTrayManager pieceTrayManager;
    private HighScoreManager highScoreManager;
    private bool isGameOver;
    [Header("Game Over Animation")]
    [SerializeField] private float panelStartScale = 0.90f;
    [SerializeField] private float panelAnimationDuration = 0.35f;

    private CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        comboManager = FindFirstObjectByType<ComboManager>();
        boardManager = FindFirstObjectByType<BoardManager>();
        pieceTrayManager = FindFirstObjectByType<PieceTrayManager>();
        highScoreManager = FindFirstObjectByType<HighScoreManager>(); 
        if (gameOverPanel != null)
        {
            gameOverCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        }
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
        int bestScore =  highScoreManager.GetBestScore();
        bool hasNewHighScore = highScoreManager.HasNewHighScoreThisGame();
        if (finalScoreText != null)
        {
            finalScoreText.text ="SCORE\n" +finalScore;
        }
        if (bestScoreText != null)
        {
            bestScoreText.text ="BEST\n" + bestScore;
        }
        if (newRecordText != null)
        {
            newRecordText.gameObject.SetActive(hasNewHighScore);
        }
        gameOverPanel.SetActive(true);
        PlayGameOverAnimation();
        Debug.Log("GAME OVER | Final Score: " + finalScore +" | Best Score: " + bestScore);
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
            gameOverPanel.transform.DOKill();
            if (gameOverCanvasGroup != null)
            {
                gameOverCanvasGroup.DOKill();
                gameOverCanvasGroup.alpha = 1f;
            }
            gameOverPanel.transform.localScale = Vector3.one;
            gameOverPanel.SetActive(false);
        }
        Debug.Log("Yeni oyun başladı.");
    }
    private void PlayGameOverAnimation()
    {
        if (gameOverPanel == null)
        {
            return;
        }
        gameOverPanel.transform.DOKill();
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.DOKill();
        }
        gameOverPanel.transform.localScale = Vector3.one * panelStartScale;
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
        }
        gameOverPanel.transform.DOScale(Vector3.one,panelAnimationDuration).SetEase(Ease.OutBack);
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.DOFade(1f,panelAnimationDuration);
        }
    }
}