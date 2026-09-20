using UnityEngine;

public class PieceTrayManager : MonoBehaviour
{
    [Header("Piece Prefab")]
    public Piece piecePrefab;

    [Header("Piece Slots")]
    public Transform[] pieceSlots;

    [Header("Available Pieces")]
    public PieceDataSO[] availablePieces;

    private void Start()
    {
        CreateNewPieces();
    }

    private void CreateNewPieces()
    {
        for (int i = 0; i < pieceSlots.Length; i++)
        {
            PieceDataSO randomPieceData = GetRandomPiece();

            Piece newPiece = Instantiate(piecePrefab,pieceSlots[i]);

            RectTransform pieceRect = newPiece.GetComponent<RectTransform>();

            pieceRect.anchoredPosition = Vector2.zero;

            pieceRect.localScale = Vector3.one;

            newPiece.Setup(randomPieceData);
        }
    }

    private PieceDataSO GetRandomPiece()
    {
        int randomIndex = Random.Range(0,availablePieces.Length);

        return availablePieces[randomIndex];
    }
}