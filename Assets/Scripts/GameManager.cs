using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI moneyText;

    [Header("Player Stats")]
    public int money;
    public int level;
    public float exp;

    [Header("Stage Info")]
    public int currentStage;
    public int monstersRemaining;

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
        moneyText.text = money.ToString();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
    }

    public void AddExp(float amount)
    {
        exp += amount;
        // handle level up if exp >= requiredExp
    }

    public void MonsterDefeated()
    {
        monstersRemaining--;
        if (monstersRemaining <= 0)
        {
            NextStage();
        }
    }

    void NextStage()
    {
        currentStage++;
        // spawn monsters or trigger next wave
    }
}
