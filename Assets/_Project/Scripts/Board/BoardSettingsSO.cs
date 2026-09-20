using UnityEngine;

[CreateAssetMenu(fileName = "BoardSettings",menuName = "Block Puzzle/Board Settings")]

public class BoardSettingsSO : ScriptableObject
{
    [Header("Board Size")]
    public int rows = 8;
    public int columns = 8;

    [Header("Cell Settings")]
    public float cellSize = 100f;
    public float spacing = 5f;

}