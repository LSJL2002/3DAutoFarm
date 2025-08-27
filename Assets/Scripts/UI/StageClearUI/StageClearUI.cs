using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageClearUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Button continueButton;

    private StageManager stageManager;
    private int pendingGold;
    private int pendingExp;

    void Awake()
    {
        panel.SetActive(false);
        continueButton.onClick.AddListener(OnContinue);
    }

    public void Init(StageManager manager)
    {
        stageManager = manager;
    }

    public void ShowRewards(int gold, int exp)
    {
        pendingGold = gold;
        pendingExp = exp;

        goldText.text = $"+{gold} Gold";
        expText.text = $"+{exp} EXP";

        panel.SetActive(true);
    }

    private void OnContinue()
    {
        panel.SetActive(false);

        // Give rewards only after pressing continue
        GameManager.Instance.AddMoney(pendingGold);
        GameManager.Instance.AddExp(pendingExp);

        stageManager.LoadNextStage();
    }
}
