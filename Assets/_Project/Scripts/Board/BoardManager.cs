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

    public bool PlacePiece(Piece piece,out int clearedLines)
    {
        clearedLines = 0;
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
        clearedLines = ClearCompletedLines();
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

    private int ClearCompletedLines()
    {
        List<BoardCell> cellsToClear = new List<BoardCell>();
        int completedLines = 0;
        completedLines += FindCompletedRows(cellsToClear);
        completedLines += FindCompletedColumns(cellsToClear);
        if (completedLines == 0)
        {
            return 0;
        }
        for (int i = 0;i < cellsToClear.Count;i++)
        {
            cellsToClear[i].ClearCell();
        }
        Debug.Log("Temizlenen çizgi sayısı: " + completedLines);
        return completedLines;
    }

    private int FindCompletedRows(List<BoardCell> cellsToClear)
    {
        int completedRows = 0;
        for (int row = 0;row < boardSettings.rows;row++)
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
                completedRows++;

                Debug.Log("Satır " +(row + 1) +" temizlenecek!");

                for (int column = 0;column < boardSettings.columns;column++)
                {
                    BoardCell cell = boardCells[row, column];
                    if (!cellsToClear.Contains(cell))
                    {
                        cellsToClear.Add(cell);
                    }
                }
            }
        }
        return completedRows;
    }

    private int FindCompletedColumns(List<BoardCell> cellsToClear)
    {
        int completedColumns = 0;
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
                completedColumns++;
                Debug.Log("Sütun " +(column + 1) + " temizlenecek!");

                for (int row = 0;row < boardSettings.rows;row++)
                {
                    BoardCell cell = boardCells[row, column];
                    if (!cellsToClear.Contains(cell))
                    {
                        cellsToClear.Add(cell);
                    }
                }
            }
        }
        return completedColumns;
    }
    public bool CanPieceFitAnywhere(PieceDataSO pieceData)
    {
        if (pieceData == null)
        {
            return false;
        }
        if (pieceData.cells == null || pieceData.cells.Length == 0)
        {
            return false;
        }
        for (int row = 0;row < boardSettings.rows;row++)
        {
            for (int column = 0;column < boardSettings.columns;column++)
            {
                bool canFit = CanPieceFitAt(pieceData,row,column);
                if (canFit)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanPieceFitAt(PieceDataSO pieceData,int startRow,int startColumn)
    {
        int minX = pieceData.cells[0].x;
        int maxY = pieceData.cells[0].y;
        for (int i = 0;i < pieceData.cells.Length;i++)
        {
            Vector2Int cell = pieceData.cells[i];
            if (cell.x < minX)
            {
                minX = cell.x;
            }
            if (cell.y > maxY)
            {
                maxY = cell.y;
            }
        }
        for (int i = 0;i < pieceData.cells.Length;i++)
        {
            Vector2Int cell = pieceData.cells[i];
            int columnOffset = cell.x - minX;
            int rowOffset = maxY - cell.y;
            int targetRow = startRow + rowOffset;
            int targetColumn = startColumn + columnOffset;
            if (targetRow < 0 || targetRow >= boardSettings.rows || targetColumn < 0 || targetColumn >= boardSettings.columns)
            {
                return false;
            }
            if (boardCells[targetRow,targetColumn].isOccupied)
            {
                return false;
            }
        }
        return true;
    }
}