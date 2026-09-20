using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Piece Data")]
    public PieceDataSO pieceData;

    [Header("References")]
    public RectTransform blockPrefab;

    [Header("Settings")]
    public float blockSize = 100f;

    public void Setup(PieceDataSO newPieceData)
    {
        pieceData = newPieceData;

        CreatePiece();
    }

    private void CreatePiece()
    {
        if (pieceData == null)
        {
            Debug.LogWarning("Piece Data bulunamadı!");
            return;
        }

        if (pieceData.cells.Length == 0)
        {
            Debug.LogWarning("Piece içerisinde cell bulunamadı!");
            return;
        }

        float minX = pieceData.cells[0].x;
        float maxX = pieceData.cells[0].x;

        float minY = pieceData.cells[0].y;
        float maxY = pieceData.cells[0].y;

        for (int i = 0; i < pieceData.cells.Length; i++)
        {
            Vector2Int cell = pieceData.cells[i];

            if (cell.x < minX)
            {
                minX = cell.x;
            }

            if (cell.x > maxX)
            {
                maxX = cell.x;
            }

            if (cell.y < minY)
            {
                minY = cell.y;
            }

            if (cell.y > maxY)
            {
                maxY = cell.y;
            }
        }

        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;

        for (int i = 0; i < pieceData.cells.Length; i++)
        {
            CreateBlock(pieceData.cells[i],centerX,centerY);
        }
    }

    private void CreateBlock(Vector2Int cellPosition,float centerX,float centerY)
    {
        RectTransform newBlock = Instantiate(blockPrefab,transform);

        float xPosition = (cellPosition.x - centerX) * blockSize;

        float yPosition = (cellPosition.y - centerY) * blockSize;

        newBlock.anchoredPosition = new Vector2(xPosition,yPosition);
    }
}