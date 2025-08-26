using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public Image levelBar;

    void Update()
    {
        levelBar.fillAmount = GetPercentage();
    }
    private float GetPercentage()
    {
        return maxValue > 0 ? curValue / maxValue : 0f;
    }
}
