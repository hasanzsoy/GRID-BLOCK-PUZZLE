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

    [Header("Generation Settings")]
    public PieceGenerationSettingsSO generationSettings;

    [Header("Tray Visual Settings")]
    [SerializeField] private float trayPieceScale = 0.75f;

    private int remainingPieces;

    private List<Piece> activePieces =
        new List<Piece>();

    private BoardManager boardManager;
    private GameOverManager gameOverManager;


    private void Awake()
    {
        boardManager =
            FindFirstObjectByType<BoardManager>();

        gameOverManager =
            FindFirstObjectByType<GameOverManager>();
    }


    private void Start()
    {
        CreateNewPieces();
    }


    private void CreateNewPieces()
    {
        activePieces.Clear();

        int largePiecesInBatch = 0;

        for (int i = 0;
             i < pieceSlots.Length;
             i++)
        {
            PieceDataSO randomPieceData =
                GetWeightedRandomPiece(
                    largePiecesInBatch
                );

            if (randomPieceData == null)
            {
                Debug.LogError(
                    "Piece oluşturulamadı! " +
                    "Available Pieces listesini kontrol et."
                );

                continue;
            }

            // Large Piece geldiyse
            // mevcut batch sayısını artır.
            if (randomPieceData.sizeCategory ==
                PieceSizeCategory.Large)
            {
                largePiecesInBatch++;
            }

            Piece newPiece =
                Instantiate(
                    piecePrefab,
                    pieceSlots[i]
                );

            RectTransform pieceRect =
                newPiece.GetComponent<RectTransform>();

            pieceRect.anchoredPosition =
                Vector2.zero;

            pieceRect.localScale =
                Vector3.one * trayPieceScale;

            newPiece.Setup(
                randomPieceData
            );

            activePieces.Add(
                newPiece
            );

            Debug.Log(
                "Yeni Piece: " +
                randomPieceData.name +
                " | Kategori: " +
                randomPieceData.sizeCategory
            );
        }

        remainingPieces =
            activePieces.Count;

        Debug.Log(
            "Yeni parça grubu oluşturuldu. " +
            "Parça sayısı: " +
            remainingPieces
        );
    }


    private PieceDataSO GetWeightedRandomPiece(
        int largePiecesInBatch)
    {
        // Settings atanmadıysa
        // eski random sistemine dön.
        if (generationSettings == null)
        {
            return GetAnyRandomPiece(true);
        }

        int smallWeight =
            generationSettings.smallWeight;

        int mediumWeight =
            generationSettings.mediumWeight;

        int largeWeight =
            generationSettings.largeWeight;


        // Bu batch için Large limiti dolduysa
        // Large ihtimalini kapat.
        if (largePiecesInBatch >=
            generationSettings
                .maxLargePiecesPerBatch)
        {
            largeWeight = 0;
        }


        int totalWeight =
            smallWeight +
            mediumWeight +
            largeWeight;


        // Yanlışlıkla bütün ağırlıklar 0 yapılırsa
        // oyun bozulmasın.
        if (totalWeight <= 0)
        {
            return GetAnyRandomPiece(
                largeWeight > 0
            );
        }


        int randomValue =
            Random.Range(
                0,
                totalWeight
            );


        PieceSizeCategory selectedCategory;


        if (randomValue < smallWeight)
        {
            selectedCategory =
                PieceSizeCategory.Small;
        }
        else if (
            randomValue <
            smallWeight + mediumWeight)
        {
            selectedCategory =
                PieceSizeCategory.Medium;
        }
        else
        {
            selectedCategory =
                PieceSizeCategory.Large;
        }


        PieceDataSO selectedPiece =
            GetRandomPieceFromCategory(
                selectedCategory
            );


        // Seçilen kategoride Piece bulunamadıysa
        // güvenli fallback kullan.
        if (selectedPiece == null)
        {
            bool allowLarge =
                largeWeight > 0;

            return GetAnyRandomPiece(
                allowLarge
            );
        }


        return selectedPiece;
    }


    private PieceDataSO GetRandomPieceFromCategory(
        PieceSizeCategory category)
    {
        List<PieceDataSO> matchingPieces =
            new List<PieceDataSO>();


        for (int i = 0;
             i < availablePieces.Length;
             i++)
        {
            PieceDataSO pieceData =
                availablePieces[i];

            if (pieceData == null)
            {
                continue;
            }

            if (pieceData.sizeCategory ==
                category)
            {
                matchingPieces.Add(
                    pieceData
                );
            }
        }


        if (matchingPieces.Count == 0)
        {
            return null;
        }


        int randomIndex =
            Random.Range(
                0,
                matchingPieces.Count
            );


        return matchingPieces[
            randomIndex
        ];
    }


    private PieceDataSO GetAnyRandomPiece(
        bool allowLarge)
    {
        List<PieceDataSO> usablePieces =
            new List<PieceDataSO>();


        for (int i = 0;
             i < availablePieces.Length;
             i++)
        {
            PieceDataSO pieceData =
                availablePieces[i];

            if (pieceData == null)
            {
                continue;
            }


            if (!allowLarge &&
                pieceData.sizeCategory ==
                PieceSizeCategory.Large)
            {
                continue;
            }


            usablePieces.Add(
                pieceData
            );
        }


        // Large kapalıyken hiç Piece kalmadıysa
        // bütün listeyi tekrar dene.
        if (usablePieces.Count == 0)
        {
            for (int i = 0;
                 i < availablePieces.Length;
                 i++)
            {
                if (availablePieces[i] != null)
                {
                    usablePieces.Add(
                        availablePieces[i]
                    );
                }
            }
        }


        if (usablePieces.Count == 0)
        {
            return null;
        }


        int randomIndex =
            Random.Range(
                0,
                usablePieces.Count
            );


        return usablePieces[
            randomIndex
        ];
    }


    public void PieceUsed(
        Piece usedPiece)
    {
        activePieces.Remove(
            usedPiece
        );

        remainingPieces =
            activePieces.Count;

        Debug.Log(
            "Kalan parça sayısı: " +
            remainingPieces
        );


        // Üçlü tamamen bittiyse
        // yeni grup oluştur.
        if (remainingPieces <= 0)
        {
            CreateNewPieces();
        }


        CheckForGameOver();
    }


    private void CheckForGameOver()
    {
        for (int i = 0;
             i < activePieces.Count;
             i++)
        {
            Piece currentPiece =
                activePieces[i];

            if (currentPiece == null)
            {
                continue;
            }


            bool canFit =
                boardManager
                    .CanPieceFitAnywhere(
                        currentPiece.pieceData
                    );


            // En az bir Piece yerleşebiliyorsa
            // oyun devam eder.
            if (canFit)
            {
                Debug.Log(
                    "Oyun devam ediyor. " +
                    "En az bir parça yerleşebilir."
                );

                return;
            }
        }


        Debug.Log(
            "NO SPACE LEFT!"
        );


        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }


    public void ResetTray()
    {
        for (int i = 0;
             i < activePieces.Count;
             i++)
        {
            Piece currentPiece =
                activePieces[i];

            if (currentPiece != null)
            {
                currentPiece.gameObject
                    .SetActive(false);

                Destroy(
                    currentPiece.gameObject
                );
            }
        }


        activePieces.Clear();

        remainingPieces = 0;


        CreateNewPieces();


        Debug.Log(
            "Piece Tray sıfırlandı."
        );
    }
}