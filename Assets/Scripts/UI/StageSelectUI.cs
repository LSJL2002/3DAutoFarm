using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectUI : MonoBehaviour
{
    [Header("References")]
    public GameObject stageSelectUI;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    private StageManager stageManager;

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        if (stageManager == null)
        {
            Debug.LogError("StageManager not found in scene!");
            return;
        }

        if (stageSelectUI != null)
            stageSelectUI.SetActive(false);

        int maxStage = Mathf.Max(1, GameManager.Instance.currentStage);
        for (int i = 1; i <= maxStage; i++)
        {
            CreateButton(i);
        }
    }

    public void CreateButton(int stageNumber)
    {
        if (buttonParent == null || buttonPrefab == null) return;

        if (buttonParent.Find($"StageButton_{stageNumber}") != null) return;

        GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
        btnObj.name = $"StageButton_{stageNumber}";

        Button btn = btnObj.GetComponent<Button>();
        TextMeshProUGUI label = btnObj.GetComponentInChildren<TextMeshProUGUI>();

        if (label != null)
            label.text = $"Stage {stageNumber}";

        int stageNum = stageNumber;
        btn.onClick.AddListener(() => stageManager.LoadStageByNumber(stageNum));
    }

    public void CloseStageSelect()
    {
        if (stageSelectUI != null)
            stageSelectUI.SetActive(false);
    }
}
