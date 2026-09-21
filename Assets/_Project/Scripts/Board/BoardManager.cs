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
        List<RectTransform> blocks = piece.GetBlocks();

        List<BoardCell> detectedCells = new List<BoardCell>();

        for (int i = 0; i < blocks.Count; i++)
        {
            BoardCell detectedCell =
                GetCellUnderBlock(blocks[i]);

            if (detectedCell == null)
            {
                return false;
            }

            if (detectedCell.isOccupied)
            {
                return false;
            }

            if (detectedCells.Contains(detectedCell))
            {
                return false;
            }

            detectedCells.Add(detectedCell);
        }

        return true;
    }

    private BoardCell GetCellUnderBlock(RectTransform block)
    {
        Vector2 blockScreenPosition = RectTransformUtility.WorldToScreenPoint(null,block.position);

        for (int row = 0; row < boardSettings.rows; row++)
        {
            for (int column = 0; column < boardSettings.columns; column++)
            {
                BoardCell cell = boardCells[row, column];

                RectTransform cellRect = cell.GetComponent<RectTransform>();

                bool isInside =
                    RectTransformUtility.RectangleContainsScreenPoint(cellRect,blockScreenPosition,null);

                if (isInside)
                {
                    return cell;
                }
            }
        }

        return null;
    }
}