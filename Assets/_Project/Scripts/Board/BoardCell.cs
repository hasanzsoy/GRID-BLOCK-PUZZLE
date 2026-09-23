using UnityEngine;

public class BoardCell : MonoBehaviour
{
    public int row;
    public int column;

    public bool isOccupied;

    public RectTransform placedBlock;

    public void SetBlock(RectTransform block)
    {
        placedBlock = block;

        isOccupied = true;

        block.SetParent(transform, false);

        block.anchoredPosition = Vector2.zero;

        block.localScale = Vector3.one;
    }
}