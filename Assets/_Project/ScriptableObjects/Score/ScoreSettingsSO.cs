using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSettings",menuName = "Block Puzzle/Score Settings")]
public class ScoreSettingsSO : ScriptableObject
{
    [Header("Score Settings")]
    public int scorePerBlock = 1;
}