using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClearVFX : MonoBehaviour
{
    [Header("Spark Prefab")]
    [SerializeField] private RectTransform sparkPrefab;

    [Header("VFX Settings")]
    [SerializeField] private int sparkCount = 6;
    [SerializeField] private float minDistance = 40f;
    [SerializeField] private float maxDistance = 80f;
    [SerializeField] private float duration = 0.30f;

    public void Play(Color sparkColor)
    {
        for (int i = 0; i < sparkCount; i++)
        {
            CreateSpark(sparkColor);
        }
        Destroy(gameObject,duration + 0.1f);
    }

    private void CreateSpark(Color sparkColor)
    {
        RectTransform newSpark = Instantiate(sparkPrefab,transform);
        newSpark.anchoredPosition = Vector2.zero;
        newSpark.localScale = Vector3.one;
        Image sparkImage = newSpark.GetComponent<Image>();
        if (sparkImage != null)
        {
            sparkImage.color = sparkColor;
        }
        float randomAngle = Random.Range(0f,360f);
        float randomDistance = Random.Range(minDistance,maxDistance);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad),Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Vector2 targetPosition = direction * randomDistance;
        newSpark.DOAnchorPos(targetPosition,duration).SetEase(Ease.OutQuad);
        newSpark.DOScale(Vector3.zero,duration);
        if (sparkImage != null)
        {
            sparkImage.DOFade(0f,duration);
        }
    }
}