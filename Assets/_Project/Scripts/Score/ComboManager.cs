using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text comboText;
    private int currentCombo;
    [Header("Combo Animation")]
    [SerializeField] private float comboStartScale = 0.70f;
    [SerializeField] private float comboPopDuration = 0.25f;
    private void Start()
    {
        ResetCombo();
    }

    public void ProcessMove(int clearedLines)
    {       
        if (clearedLines > 0)
        {
            IncreaseCombo();
        }
        else
        {
            ResetCombo();
        }
    }

    private void IncreaseCombo()
    {
        currentCombo++;

        UpdateComboText();

        Debug.Log("Combo: " + currentCombo);
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        if (comboText != null)
        {
            comboText.transform.DOKill();
            comboText.transform.localScale = Vector3.one;
            comboText.text = "";
            comboText.gameObject.SetActive(false);
        }
        Debug.Log("Combo sıfırlandı.");
    }
    private void UpdateComboText()
    {
        if (comboText == null)
        {
            return;
        }
        comboText.gameObject.SetActive(true);
        comboText.text = "COMBO " + currentCombo;
        comboText.transform.DOKill();
        comboText.transform.localScale = Vector3.one * comboStartScale;
        comboText.transform.DOScale(Vector3.one,comboPopDuration).SetEase(Ease.OutBack);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}