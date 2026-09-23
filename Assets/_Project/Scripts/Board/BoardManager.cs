using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    public BoardSettingsSO boardSettings;

    [Header("References")]
    public BoardCell cellPrefab;
    public Transform boardParent;

    private BoardCell[,] boardCells;

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        boardCells = new BoardCell[boardSettings.rows,boardSettings.columns];

        for (int row = 0; row < boardSettings.rows; row++)
        {
            for (int column = 0; column < boardSettings.columns; column++)
            {
                CreateCell(row, column);
            }
        }
    }

    private void CreateCell(int row, int column)
    {
        BoardCell newCell = Instantiate(cellPrefab,boardParent);

        newCell.row = row;
        newCell.column = column;

        newCell.isOccupied = false;

        newCell.name = "Cell_" + row + "_" + column;

        boardCells[row, column] = newCell;
    }

    public bool CanPlacePiece(Piece piece)
    {
        List<BoardCell> detectedCells = GetPlacementCells(piece);

        return detectedCells != null;
    }

    public bool PlacePiece(Piece piece)
    {
        List<BoardCell> detectedCells = GetPlacementCells(piece);

        if (detectedCells == null)
        {
            return false;
        }

        List<RectTransform> blocks = piece.GetBlocks();

        for (int i = 0; i < blocks.Count; i++)
        {
            detectedCells[i].SetBlock(blocks[i]);
        }

        // Parça yerleştirildikten sonra
        // tamamlanan satır ve sütunları kontrol ediyoruz.
        CheckCompletedLines();

        Destroy(piece.gameObject);

        return true;
    }

    private List<BoardCell> GetPlacementCells(Piece piece)
    {
        List<RectTransform> blocks = piece.GetBlocks();

        List<BoardCell> detectedCells = new List<BoardCell>();

        for (int i = 0; i < blocks.Count; i++)
        {
            BoardCell detectedCell = GetCellUnderBlock(blocks[i]);

            if (detectedCell == null)
            {
                return null;
            }

            if (detectedCell.isOccupied)
            {
                return null;
            }

            if (detectedCells.Contains(detectedCell))
            {
                return null;
            }

            detectedCells.Add(detectedCell);
        }

        return detectedCells;
    }

    private BoardCell GetCellUnderBlock(RectTransform block)
    {
        Vector2 blockScreenPosition = RectTransformUtility.WorldToScreenPoint(null,block.position);

        for (int row = 0;row < boardSettings.rows;row++)
        {
            for (int column = 0;column < boardSettings.columns;column++)
            {
                BoardCell cell = boardCells[row, column];

                RectTransform cellRect = cell.GetComponent<RectTransform>();

                bool isInside = RectTransformUtility.RectangleContainsScreenPoint(cellRect,blockScreenPosition,null);

                if (isInside)
                {
                    return cell;
                }
            }
        }

        return null;
    }

    private void CheckCompletedLines()
    {
        CheckRows();
        CheckColumns();
    }

    private void CheckRows()
    {
        for (int row = 0; row < boardSettings.rows; row++)
        {
            bool isRowFull = true;

            for (int column = 0;column < boardSettings.columns;column++)
            {
                if (!boardCells[row, column].isOccupied)
                {
                    isRowFull = false;
                    break;
                }
            }

            if (isRowFull)
            {
                Debug.Log("Satır " + (row + 1) + " tamamlandı!");
            }
        }
    }

    private void CheckColumns()
    {
        for (int column = 0;column < boardSettings.columns;column++)
        {
            bool isColumnFull = true;

            for (int row = 0;row < boardSettings.rows;row++)
            {
                if (!boardCells[row, column].isOccupied)
                {
                    isColumnFull = false;
                    break;
                }
            }

            if (isColumnFull)
            {
                Debug.Log("Sütun " + (column + 1) + " tamamlandı!");
            }
        }
    }
}