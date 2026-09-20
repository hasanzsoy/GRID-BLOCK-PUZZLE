using UnityEngine;

[CreateAssetMenu(fileName = "NewPiece",menuName = "Block Puzzle/Piece Data")]
public class PieceDataSO : ScriptableObject
{
    [Header("Piece Info")]
    public string pieceName;

    [Header("Piece Shape")]
    public Vector2Int[] cells;
}