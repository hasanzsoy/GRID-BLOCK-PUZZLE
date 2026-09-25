using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    public int row;
    public int column;
    public bool isOccupied;
    public RectTransform placedBlock;
    [Header("Clear Fall Animation")]
    [SerializeField] private float popScale = 1.10f;
    [SerializeField] private float popDuration = 0.06f;
    [SerializeField] private float fallDistance = 350f;
    [SerializeField] private float fallDuration = 0.45f;
    [SerializeField] private float horizontalDrift = 100f;
    [SerializeField] private float maxRotation = 65f;
    [SerializeField] private float fadeDuration = 0.15f;
    [Header("Clear VFX")]
    [SerializeField] private ClearVFX clearVfxPrefab;

    public void SetBlock(RectTransform block)
    {
        placedBlock = block;
        isOccupied = true;
        block.SetParent(transform, false);
        block.anchoredPosition = Vector2.zero;
        block.localScale = Vector3.one;
    }

    public void ClearCell()
    {
        if (placedBlock != null)
        {
            placedBlock.DOKill();
            Image blockImage = placedBlock.GetComponent<Image>();
            if (blockImage != null)
            {
                blockImage.DOKill();
            }
            Destroy(placedBlock.gameObject);
        }
        placedBlock = null;
        isOccupied = false;
    }

    public void ClearCellAnimated()
    {
        if (placedBlock == null)
        {
            isOccupied = false;
            return;
        }
        RectTransform blockToClear = placedBlock;
        Image blockImage = blockToClear.GetComponent<Image>();
        placedBlock = null;
        isOccupied = false;
        blockToClear.DOKill();
        if (blockImage != null)
        {
            blockImage.DOKill();
        }
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Destroy(blockToClear.gameObject);
            return;
        }
        blockToClear.SetAsLastSibling();
        blockToClear.SetParent(canvas.transform,true);
        blockToClear.SetAsLastSibling();
        if (clearVfxPrefab != null)
        {
            ClearVFX newVfx = Instantiate(clearVfxPrefab,canvas.transform);
            RectTransform vfxRect = newVfx.GetComponent<RectTransform>();
            vfxRect.position = blockToClear.position;
            vfxRect.localScale = Vector3.one;
            vfxRect.SetAsLastSibling();
            newVfx.Play(Color.white);
            Debug.Log("Clear VFX çalıştı!");
        }
        Vector2 startPosition = blockToClear.anchoredPosition;
        float randomX = Random.Range(-horizontalDrift,horizontalDrift);
        float randomRotation = Random.Range(-maxRotation,maxRotation);
        float randomDelay = Random.Range(0f,0.06f);
        Vector2 targetPosition = new Vector2(startPosition.x + randomX,startPosition.y - fallDistance);
        Sequence clearSequence = DOTween.Sequence();
        clearSequence.SetDelay(randomDelay);
        clearSequence.Append(blockToClear.DOScale(Vector3.one * popScale,popDuration).SetEase(Ease.OutQuad));
        clearSequence.Append(blockToClear.DOAnchorPos(targetPosition,fallDuration).SetEase(Ease.InQuad));
        clearSequence.Join(blockToClear.DORotate(new Vector3(0f,0f,randomRotation),fallDuration).SetEase(Ease.Linear));
        clearSequence.Join(blockToClear.DOScale(Vector3.one * 0.80f,fallDuration).SetEase(Ease.InQuad));
        if (blockImage != null)
        {
            float fadeStartTime = popDuration + fallDuration - fadeDuration;
            clearSequence.Insert(fadeStartTime,blockImage.DOFade(0f,fadeDuration));
        }
        clearSequence.OnComplete(() =>{if (blockToClear != null){Destroy(blockToClear.gameObject);}});
    }
}