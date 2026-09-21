using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform startParent;
    private Vector2 startPosition;
    private Piece piece;
    private BoardManager boardManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();

        piece = GetComponent<Piece>();

        boardManager = FindFirstObjectByType<BoardManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;

        startPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);

        transform.SetAsLastSibling();

        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool canPlace = boardManager.CanPlacePiece(piece);

        if (canPlace)
        {
            Debug.Log("Geçerli Konum");
        }
        else
        {
            Debug.Log("Geçersiz Konum");
        }

        ReturnToStart();
    }

    private void ReturnToStart()
    {
        transform.SetParent(startParent, false);

        rectTransform.anchoredPosition = startPosition;

        transform.localScale = Vector3.one;
    }
}