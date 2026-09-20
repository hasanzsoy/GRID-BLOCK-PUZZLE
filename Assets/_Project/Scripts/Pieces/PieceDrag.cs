using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDrag : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform startParent;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Parçanın başlangıç slotunu kaydediyoruz.
        startParent = transform.parent;

        // Slot içerisindeki başlangıç konumunu kaydediyoruz.
        startPosition = rectTransform.anchoredPosition;

        // Drag sırasında parçayı Canvas altına alıyoruz.
        transform.SetParent(canvas.transform, true);

        // Diğer UI elemanlarının önünde görünsün.
        transform.SetAsLastSibling();

        // Tutulduğunu belli etmek için biraz büyütüyoruz.
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mouse veya parmak hareketi kadar parçayı hareket ettiriyoruz.
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Parçayı tekrar eski slotunun altına gönderiyoruz.
        transform.SetParent(startParent, false);

        // Başlangıç konumuna geri getiriyoruz.
        rectTransform.anchoredPosition = startPosition;

        // Boyutunu normale döndürüyoruz.
        transform.localScale = Vector3.one;
    }
}