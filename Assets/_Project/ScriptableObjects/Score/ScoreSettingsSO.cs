using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSettings",menuName = "Block Puzzle/Score Settings")]
public class ScoreSettingsSO : ScriptableObject
{
    [Header("Piece Score")]
    public int scorePerBlock = 1;

    [Header("Line Clear Bonus")]
    public int oneLineBonus = 10;
    public int twoLineBonus = 25;
    public int threeLineBonus = 45;
    public int fourOrMoreLineBonus = 70;
}