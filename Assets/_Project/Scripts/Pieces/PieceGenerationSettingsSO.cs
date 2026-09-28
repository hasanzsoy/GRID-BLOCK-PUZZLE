using UnityEngine;

[CreateAssetMenu(fileName = "PieceGenerationSettings",menuName = "Block Puzzle/Piece Generation Settings")]
public class PieceGenerationSettingsSO : ScriptableObject
{
    [Header("Category Weights")]
    [Min(0)]
    public int smallWeight = 40;
    [Min(0)]
    public int mediumWeight = 45;
    [Min(0)]
    public int largeWeight = 15;
    [Header("Batch Rules")]
    [Min(0)]
    public int maxLargePiecesPerBatch = 1;
    [Header("Anti Repeat Rules")]
    public bool avoidSamePieceInSameBatch = true;
    public bool avoidPreviousBatchPieces = true;
}