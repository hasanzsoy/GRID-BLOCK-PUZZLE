using System.Collections.Generic;
using UnityEngine;

public class PieceTrayManager : MonoBehaviour
{
    [Header("Piece Prefab")]
    public Piece piecePrefab;

    [Header("Piece Slots")]
    public Transform[] pieceSlots;

    [Header("Available Pieces")]
    public PieceDataSO[] availablePieces;

    private int remainingPieces;

    private List<Piece> activePieces =
        new List<Piece>();

    private BoardManager boardManager;
    private GameOverManager gameOverManager;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();

        gameOverManager = FindFirstObjectByType<GameOverManager>();
    }

    private void Start()
    {
        CreateNewPieces();
    }

    private void CreateNewPieces()
    {
        activePieces.Clear();

        for (int i = 0;i < pieceSlots.Length;i++)
        {
            PieceDataSO randomPieceData = GetRandomPiece();
            Piece newPiece = Instantiate(piecePrefab,pieceSlots[i]);
            RectTransform pieceRect = newPiece.GetComponent<RectTransform>();
            pieceRect.anchoredPosition = Vector2.zero;
            pieceRect.localScale = Vector3.one;
            newPiece.Setup(randomPieceData);
            activePieces.Add(newPiece);
        }
        remainingPieces = activePieces.Count;
        Debug.Log("Yeni parça grubu oluşturuldu. " +"Parça sayısı: " +remainingPieces);
    }

    private PieceDataSO GetRandomPiece()
    {
        int randomIndex = Random.Range(0,availablePieces.Length);
        return availablePieces[randomIndex];
    }
    public void PieceUsed(Piece usedPiece)
    {
        activePieces.Remove(usedPiece);
        remainingPieces = activePieces.Count;
        Debug.Log("Kalan parça sayısı: " +remainingPieces);
        if (remainingPieces <= 0)
        {
            CreateNewPieces();
        }
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
         for (int i = 0;i < activePieces.Count;i++)
        {
            Piece currentPiece = activePieces[i];

            if (currentPiece == null)
            {
                continue;
            }
            bool canFit = boardManager.CanPieceFitAnywhere(currentPiece.pieceData);
            if (canFit)
            {
                Debug.Log("Oyun devam ediyor. " + "En az bir parça yerleşebilir.");
                return;
            }
        }

        Debug.Log("NO SPACE LEFT!");

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }
}