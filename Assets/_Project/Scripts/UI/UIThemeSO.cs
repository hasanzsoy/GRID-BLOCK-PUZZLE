using UnityEngine;

[CreateAssetMenu(fileName = "UITheme",menuName = "Block Puzzle/UI Theme")]
public class UIThemeSO : ScriptableObject
{
    [Header("Main Colors")]
    public Color backgroundColor;
    public Color boardColor;
    public Color emptyCellColor;
    public Color pieceColor;
    [Header("Text Colors")]
    public Color primaryTextColor;
    public Color secondaryTextColor;
    public Color successTextColor;
    [Header("Game Over")]
    public Color gameOverOverlayColor;
    public Color buttonColor;
    public Color buttonTextColor;
}