using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text comboText;
    private int currentCombo;
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
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}