using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Idle,
    Chasing,
    Attacking
}

public class MonsterActions : MonoBehaviour, IDamageable
{
    private Renderer[] rend;
    private Color[] originalColor;
    [SerializeField] private float flashDuration = 0.2f;
    private StatHandler stats;
    private NavMeshAgent agent;
    private GameObject player;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private float lastAttackTime = 0f;

    [Header("Drop loot")]
    [SerializeField] private int goldAmount = 5;
    [SerializeField] private int expAmount = 10;
    private StageManager stageManager;

    private MonsterState currentState = MonsterState.Idle;

    void Start()
    {
        stats = GetComponent<StatHandler>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        stageManager = FindObjectOfType<StageManager>();

        rend = GetComponentsInChildren<Renderer>();
        originalColor = new Color[rend.Length];
        for (int i = 0; i < rend.Length; i++)
        {
            originalColor[i] = rend[i].material.color;
        }
        if (stats != null && agent != null)
            {
                agent.speed = stats.GetStat(StatType.Speed);
            }
    }

    void Update()
    {
        if (player == null || stats == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Determine state
        if (distance <= attackRange)
            currentState = MonsterState.Attacking;
        else if (distance <= chaseRange)
            currentState = MonsterState.Chasing;
        else
            currentState = MonsterState.Idle;

        // State behavior
        switch (currentState)
        {
            case MonsterState.Idle:
                agent.isStopped = true;
                break;

            case MonsterState.Chasing:
                MoveTowardsPlayer();
                RotateTowards(player.transform.position);
                break;

            case MonsterState.Attacking:
                agent.isStopped = true;
                RotateTowards(player.transform.position);
                AttackPlayer();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null || agent == null) return;

        agent.isStopped = false;
        bool result = agent.SetDestination(player.transform.position);
    }


    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        if (direction.magnitude == 0f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        float attackSpeed = stats.GetStat(StatType.AttackSpeed);
        float attackCooldown = attackSpeed > 0 ? 1f / attackSpeed : 1f;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerCondition playerCondition = player.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                float damage = stats.GetStat(StatType.Damage);
                playerCondition.TakeDamage(damage); // this reduces player's CurrentHealth
            }
            lastAttackTime = Time.time;
        }
    }

    // IDamageable implementation
    public void TakeDamage(float damage)
    {
        if (stats != null)
        {
            stats.TakeDamage(damage);
        }

        StartCoroutine(FlashRed());

        if (stats.CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(flashDuration);
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].material.color = originalColor[i];
        }
    }

    private void Die()
    {
        GameManager.Instance.SpawnLoot(transform.position, goldAmount, expAmount);
        if (stageManager != null)
        {
            stageManager.MonsterDefeated(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
