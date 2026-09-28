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

    private List<PieceDataSO> previousBatchPieces =
        new List<PieceDataSO>();


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

        List<PieceDataSO> currentBatchPieces =
            new List<PieceDataSO>();

        int largePiecesInBatch = 0;


        Debug.Log(
            "---------- YENİ BATCH ----------"
        );


        for (int i = 0;
             i < pieceSlots.Length;
             i++)
        {
            PieceDataSO selectedPieceData =
                GetWeightedRandomPiece(
                    currentBatchPieces,
                    largePiecesInBatch
                );


            if (selectedPieceData == null)
            {
                Debug.LogError(
                    "Uygun Piece bulunamadı!"
                );

                continue;
            }


            currentBatchPieces.Add(
                selectedPieceData
            );


            if (selectedPieceData.sizeCategory ==
                PieceSizeCategory.Large)
            {
                largePiecesInBatch++;
            }


            CreatePiece(
                selectedPieceData,
                pieceSlots[i]
            );


            Debug.Log("Yeni Piece: " + selectedPieceData.name +" | ID: " + selectedPieceData.GetInstanceID() + " | Kategori: " + selectedPieceData.sizeCategory);
        }


        remainingPieces =
            activePieces.Count;


        // Bu batch artık bir sonraki batch için
        // previous batch olacak.
        previousBatchPieces.Clear();

        previousBatchPieces.AddRange(
            currentBatchPieces
        );


        Debug.Log(
            "Yeni parça grubu oluşturuldu. " +
            "Parça sayısı: " +
            remainingPieces
        );
    }


    private void CreatePiece(
        PieceDataSO pieceData,
        Transform slot)
    {
        Piece newPiece =
            Instantiate(
                piecePrefab,
                slot
            );


        RectTransform pieceRect =
            newPiece.GetComponent<RectTransform>();


        pieceRect.anchoredPosition =
            Vector2.zero;

        pieceRect.localScale =
            Vector3.one * trayPieceScale;


        newPiece.Setup(
            pieceData
        );


        activePieces.Add(
            newPiece
        );
    }


    private PieceDataSO GetWeightedRandomPiece(
        List<PieceDataSO> currentBatchPieces,
        int largePiecesInBatch)
    {
        List<PieceDataSO> smallPieces =
            new List<PieceDataSO>();

        List<PieceDataSO> mediumPieces =
            new List<PieceDataSO>();

        List<PieceDataSO> largePieces =
            new List<PieceDataSO>();


        // Önce gerçekten kullanılabilir
        // Piece'leri belirliyoruz.
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


            if (!CanUsePiece(
                    pieceData,
                    currentBatchPieces,
                    largePiecesInBatch))
            {
                continue;
            }


            if (pieceData.sizeCategory ==
                PieceSizeCategory.Small)
            {
                smallPieces.Add(
                    pieceData
                );
            }
            else if (
                pieceData.sizeCategory ==
                PieceSizeCategory.Medium)
            {
                mediumPieces.Add(
                    pieceData
                );
            }
            else
            {
                largePieces.Add(
                    pieceData
                );
            }
        }


        // Normal şartlarda buraya düşmememiz lazım.
        // Eğer gerçekten hiçbir aday kalmazsa
        // güvenlik amacıyla previous batch filtresini
        // bir kez gevşetiyoruz.
        if (smallPieces.Count == 0 &&
            mediumPieces.Count == 0 &&
            largePieces.Count == 0)
        {
            Debug.LogWarning(
                "Anti-Repeat filtreleri nedeniyle " +
                "uygun Piece kalmadı. " +
                "Previous Batch kuralı geçici olarak gevşetildi."
            );

            return GetEmergencyPiece(
                currentBatchPieces,
                largePiecesInBatch
            );
        }


        int smallWeight = 0;
        int mediumWeight = 0;
        int largeWeight = 0;


        if (smallPieces.Count > 0)
        {
            smallWeight =
                generationSettings.smallWeight;
        }


        if (mediumPieces.Count > 0)
        {
            mediumWeight =
                generationSettings.mediumWeight;
        }


        if (largePieces.Count > 0)
        {
            largeWeight =
                generationSettings.largeWeight;
        }


        int totalWeight =
            smallWeight +
            mediumWeight +
            largeWeight;


        // Inspector'da yanlışlıkla bütün
        // weight değerleri 0 yapılırsa oyun bozulmasın.
        if (totalWeight <= 0)
        {
            List<PieceDataSO> allUsablePieces =
                new List<PieceDataSO>();

            allUsablePieces.AddRange(
                smallPieces
            );

            allUsablePieces.AddRange(
                mediumPieces
            );

            allUsablePieces.AddRange(
                largePieces
            );


            return allUsablePieces[
                Random.Range(
                    0,
                    allUsablePieces.Count
                )
            ];
        }


        int randomValue =
            Random.Range(
                0,
                totalWeight
            );


        if (randomValue < smallWeight)
        {
            return GetRandomFromList(
                smallPieces
            );
        }


        randomValue -=
            smallWeight;


        if (randomValue < mediumWeight)
        {
            return GetRandomFromList(
                mediumPieces
            );
        }


        return GetRandomFromList(
            largePieces
        );
    }


    private bool CanUsePiece(
        PieceDataSO pieceData,
        List<PieceDataSO> currentBatchPieces,
        int largePiecesInBatch)
    {
        // Aynı Piece mevcut üçlüde
        // zaten seçildiyse tekrar kullanma.
        if (generationSettings
                .avoidSamePieceInSameBatch &&
            currentBatchPieces.Contains(
                pieceData))
        {
            return false;
        }


        // Önceki üçlüde bulunan Piece
        // hemen sonraki üçlüde tekrar gelmesin.
        if (generationSettings
                .avoidPreviousBatchPieces &&
            previousBatchPieces.Contains(
                pieceData))
        {
            return false;
        }


        // Large Piece limiti dolduysa
        // başka Large seçme.
        if (pieceData.sizeCategory ==
                PieceSizeCategory.Large &&
            largePiecesInBatch >=
                generationSettings
                    .maxLargePiecesPerBatch)
        {
            return false;
        }


        return true;
    }


    private PieceDataSO GetRandomFromList(
        List<PieceDataSO> pieces)
    {
        if (pieces.Count == 0)
        {
            return null;
        }


        int randomIndex =
            Random.Range(
                0,
                pieces.Count
            );


        return pieces[
            randomIndex
        ];
    }


    private PieceDataSO GetEmergencyPiece(
        List<PieceDataSO> currentBatchPieces,
        int largePiecesInBatch)
    {
        List<PieceDataSO> emergencyPieces =
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


            // Aynı batch'teki Piece tekrar etmesin.
            if (generationSettings
                    .avoidSamePieceInSameBatch &&
                currentBatchPieces.Contains(
                    pieceData))
            {
                continue;
            }


            // Large limiti yine korunuyor.
            if (pieceData.sizeCategory ==
                    PieceSizeCategory.Large &&
                largePiecesInBatch >=
                    generationSettings
                        .maxLargePiecesPerBatch)
            {
                continue;
            }


            emergencyPieces.Add(
                pieceData
            );
        }


        if (emergencyPieces.Count == 0)
        {
            return null;
        }


        return emergencyPieces[
            Random.Range(
                0,
                emergencyPieces.Count
            )
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

        // Yeni oyun önceki oyunun
        // batch geçmişini taşımamalı.
        previousBatchPieces.Clear();


        CreateNewPieces();


        Debug.Log(
            "Piece Tray sıfırlandı."
        );
    }
}