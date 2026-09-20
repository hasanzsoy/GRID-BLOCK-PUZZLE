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
}