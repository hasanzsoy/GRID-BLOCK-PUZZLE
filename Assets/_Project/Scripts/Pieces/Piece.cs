using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Piece Data")]
    public PieceDataSO pieceData;

    [Header("References")]
    public RectTransform blockPrefab;

    [Header("Settings")]
    public float blockSize = 100f;

    private void Start()
    {
        CreatePiece();
    }

    private void CreatePiece()
    {
        for (int i = 0; i < pieceData.cells.Length; i++)
        {
            CreateBlock(pieceData.cells[i]);
        }
    }

    private void CreateBlock(Vector2Int cellPosition)
    {
        RectTransform newBlock = Instantiate(blockPrefab,transform);

        float xPosition = cellPosition.x * blockSize;
        float yPosition = cellPosition.y * blockSize;

        newBlock.anchoredPosition = new Vector2(xPosition,yPosition);
    }
}