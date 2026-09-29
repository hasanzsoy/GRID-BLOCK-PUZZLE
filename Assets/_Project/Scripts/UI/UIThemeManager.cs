using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(100)]
public class UIThemeManager : MonoBehaviour
{
    [Header("Theme")]
    [SerializeField] private UIThemeSO currentTheme;

    [Header("References")]
    [SerializeField] private Camera backgroundCamera;
    [SerializeField] private Image boardImage;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;

    public UIThemeSO CurrentTheme
    {
        get
        {
            return currentTheme;
        }
    }

    private void Start()
    {
        ApplyBackgroundColor();
        ApplyBoardColor();
        ApplyEmptyCellColor();
        ApplyPrimaryTextColor();
    }

    private void ApplyBackgroundColor()
    {
        if (currentTheme == null)
        {
            return;
        }

        if (backgroundCamera == null)
        {
            return;
        }

        backgroundCamera.backgroundColor = currentTheme.backgroundColor;
    }

    private void ApplyBoardColor()
    {
        if (currentTheme == null)
        {
            return;
        }

        if (boardImage == null)
        {
            return;
        }

        boardImage.color = currentTheme.boardColor;
    }

    private void ApplyEmptyCellColor()
    {
        if (currentTheme == null)
        {
            return;
        }

        if (boardImage == null)
        {
            return;
        }

        BoardCell[] boardCells = boardImage.GetComponentsInChildren<BoardCell>(
                true
            );

        for (int i = 0;i < boardCells.Length;i++)
        {
            Image cellImage = boardCells[i].GetComponent<Image>();

            if (cellImage == null)
            {
                continue;
            }

            cellImage.color = currentTheme.emptyCellColor;
        }

        Debug.Log("Tema uygulandı. Hücre sayısı: " + boardCells.Length);
    }
    public Color GetPieceColor()
    {
        if (currentTheme == null)
        {
            return Color.white;
        }

        return currentTheme.pieceColor;
    }
    private void ApplyPrimaryTextColor()
    {
        if (currentTheme == null)
        {
            return;
        }

        if (scoreText != null)
        {
            scoreText.color = currentTheme.primaryTextColor;
        }

        if (comboText != null)
        {
            comboText.color = currentTheme.primaryTextColor;
        }
    }
}