using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")]
    public LevelBar levelBar;

    [Header("Money and Exp")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyTextShop;
    public TextMeshProUGUI levelText;

    [Header("Monster and Stage Number")]
    public TextMeshProUGUI monsterLeftText;
    public TextMeshProUGUI stageNumberText;

    [Header("Monster Drops")]
    public GameObject goldPrefab;
    public GameObject expPrefab;

    [Header("Player Stats")]
    public int money;
    public int level;
    public float exp;
    private int requiredExp = 100;

    [Header("Stage Info")]
    public int currentStage;
    public int monstersRemaining;

    private StageManager stageManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();

        if (stageManager != null)
        {
            stageManager.OnStageUpdated += UpdateStageInfo;
            // Optional: update UI initially
            UpdateStageInfo(stageManager.CurrentStageIndex + 1, stageManager.MonstersRemaining);
        }

        if (moneyText == null)
            moneyText = GameObject.Find("MoneyText")?.GetComponent<TextMeshProUGUI>();
        if (levelText == null)
            levelText = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
        if (levelBar == null)
            levelBar = GameObject.Find("LevelBar")?.GetComponent<LevelBar>();

        moneyText.text = money.ToString();
        levelText.text = level.ToString();
        moneyTextShop.text = money.ToString();
    }

    private void UpdateStageInfo(int stageNumber, int monstersLeft)
    {
        currentStage = stageNumber;
        monstersRemaining = monstersLeft;

        stageNumberText.text = $"Stage: {stageNumber}";
        monsterLeftText.text = $"Monsters: {monstersLeft}";
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
        moneyTextShop.text = money.ToString();
    }

    public void TakeMoney(int amount)
    {
        money -= amount;
        moneyText.text = money.ToString();
        moneyTextShop.text = money.ToString();
    }

    public void AddExp(float amount)
    {
        exp += amount;
        UpdateLevelBar();

        if (exp >= requiredExp)
            Levelup();
    }

    public void Levelup()
    {
        while (exp >= requiredExp)
        {
            exp -= requiredExp;
            level += 1;
            levelText.text = level.ToString();
            requiredExp = CalculateRequiredExp(level);
        }
        UpdateLevelBar();
    }
    private void UpdateLevelBar()
    {
        if (levelBar != null)
        {
            levelBar.curValue = exp;
            levelBar.maxValue = requiredExp;
        }
    }
    private int CalculateRequiredExp(int currentLevel)
    {
        return 100 + currentLevel * 20;
    }

    public void SpawnLoot(Vector3 position, int goldAmount, int expAmount)
    {
        int goldObjects = Mathf.Clamp(goldAmount, 1, 10);
        int expObjects = Mathf.Clamp(expAmount, 1, 10);

        // Spawn gold
        if (goldPrefab != null)
        {
            int goldPerObject = Mathf.Max(1, goldAmount / goldObjects); 
            for (int i = 0; i < goldObjects; i++)
            {
                GameObject gold = Instantiate(goldPrefab, position, Quaternion.identity);
                gold.GetComponent<Pickup>().Init(goldPerObject, PickupType.Gold);
            }
        }

        // Spawn exp
        if (expPrefab != null)
        {
            int expPerObject = Mathf.Max(1, expAmount / expObjects);
            for (int i = 0; i < expObjects; i++)
            {
                GameObject exp = Instantiate(expPrefab, position, Quaternion.identity);
                exp.GetComponent<Pickup>().Init(expPerObject, PickupType.Exp);
            }
        }
    }
}
