using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI moneyText;


    [Header("Monster Drops")]
    public GameObject goldPrefab;
    public GameObject expPrefab;

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
