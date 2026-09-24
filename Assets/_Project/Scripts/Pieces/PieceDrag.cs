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
    private PieceTrayManager pieceTrayManager;
    private ScoreManager scoreManager;
    private ComboManager comboManager;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        piece = GetComponent<Piece>();
        boardManager = FindFirstObjectByType<BoardManager>();
        pieceTrayManager = FindFirstObjectByType<PieceTrayManager>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        comboManager = FindFirstObjectByType<ComboManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startPosition = rectTransform.anchoredPosition;
        transform.SetParent(canvas.transform,true);
        transform.SetAsLastSibling();
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int blockCount =  piece.GetBlockCount();
        int clearedLines;
        bool piecePlaced = boardManager.PlacePiece(piece,out clearedLines);


        if (piecePlaced)
        {
            Debug.Log("Parça board'a yerleştirildi.");
            scoreManager.AddPieceScore(blockCount);
            scoreManager.AddLineClearScore(clearedLines);
            comboManager.ProcessMove(clearedLines);
            int currentCombo = comboManager.GetCurrentCombo();
            scoreManager.AddComboScore(currentCombo);
            pieceTrayManager.PieceUsed(piece);
        }
        else
        {
            Debug.Log("Geçersiz konum.");
            ReturnToStart();
        }
    }

    private void ReturnToStart()
    {
        transform.SetParent(startParent,false);
        rectTransform.anchoredPosition = startPosition;
        transform.localScale = Vector3.one;
    }
}