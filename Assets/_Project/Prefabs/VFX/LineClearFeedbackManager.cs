using DG.Tweening;
using TMPro;
using UnityEngine;
public class LineClearFeedbackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private RectTransform boardRect;
    [Header("Text Animation")]
    [SerializeField] private float startScale = 0.70f;
    [SerializeField] private float popDuration = 0.18f;
    [SerializeField] private float stayDuration = 0.25f;
    [SerializeField] private float fadeDuration = 0.22f;
    [Header("Board Feedback")]
    [SerializeField] private float boardPunchPerLine = 0.0125f;
    [SerializeField] private float boardPunchDuration = 0.18f;
    private CanvasGroup feedbackCanvasGroup;

    private void Awake()
    {
        if (feedbackText != null)
        {
            feedbackCanvasGroup =feedbackText.GetComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        ResetFeedback();
    }
    public void Play(int clearedLines)
    {
        if (clearedLines <= 0)
        {
            return;
        }

        if (feedbackText == null)
        {
            return;
        }
        feedbackText.text = GetClearMessage(clearedLines);
        feedbackText.gameObject.SetActive(true);
        feedbackText.transform.DOKill();
        if (feedbackCanvasGroup != null)
        {
            feedbackCanvasGroup.DOKill();
            feedbackCanvasGroup.alpha = 1f;
        }

        if (boardRect != null)
        {
            boardRect.DOKill();
        }
        feedbackText.transform.localScale = Vector3.one * startScale;
        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(feedbackText.transform.DOScale(Vector3.one,popDuration).SetEase(Ease.OutBack));
        textSequence.AppendInterval(stayDuration);
        if (feedbackCanvasGroup != null)
        {
            textSequence.Append(feedbackCanvasGroup.DOFade(0f,fadeDuration));
        }
        else
        {
            textSequence.AppendInterval(fadeDuration);
        }

        textSequence.OnComplete(() =>
            {
                if (feedbackText != null)
                {
                    feedbackText.transform.localScale = Vector3.one;
                    feedbackText.gameObject.SetActive(false);
                }
            }
        );

        PlayBoardPunch(clearedLines);
    }

    private void PlayBoardPunch(int clearedLines)
    {
        if (boardRect == null)
        {
            return;
        }

        int safeLineCount = Mathf.Clamp(clearedLines,1,4);
        float punchStrength = boardPunchPerLine * safeLineCount;
        boardRect.localScale = Vector3.one;
        boardRect.DOPunchScale(new Vector3(punchStrength,punchStrength,0f),boardPunchDuration,6,0.5f);
    }

    private string GetClearMessage(int clearedLines)
    {
        if (clearedLines == 1)
        {
            return "CLEAR!";
        }

        if (clearedLines == 2)
        {
            return "DOUBLE CLEAR!";
        }

        if (clearedLines == 3)
        {
            return "TRIPLE CLEAR!";
        }

        return "MEGA CLEAR!";
    }

    public void ResetFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.transform.DOKill();
            feedbackText.transform.localScale = Vector3.one;
            feedbackText.gameObject.SetActive(false);
        }

        if (feedbackCanvasGroup != null)
        {
            feedbackCanvasGroup.DOKill();
            feedbackCanvasGroup.alpha = 1f;
        }

        if (boardRect != null)
        {
            boardRect.DOKill();
            boardRect.localScale = Vector3.one;
        }
    }
}