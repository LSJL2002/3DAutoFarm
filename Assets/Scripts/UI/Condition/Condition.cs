using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public StatType statType;
    public float curValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return maxValue > 0 ? curValue / maxValue : 0f;
    }

    public void SetValues(float current, float max)
    {
        curValue = current;
        maxValue = max;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0f);
    }
}
