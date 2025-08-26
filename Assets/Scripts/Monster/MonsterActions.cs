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
    [SerializeField] private float flashDuration = 0.5f;
    private StatHandler stats;
    private NavMeshAgent agent;
    private GameObject player;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float chaseRange = 5f; // distance to start moving toward player
    [SerializeField] private float rotationSpeed = 5f;
    private float lastAttackTime = 0f;

    private MonsterState currentState = MonsterState.Idle;

    void Start()
    {
        stats = GetComponent<StatHandler>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

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
        Debug.Log($"{gameObject.name} SetDestination result: {result}, agent.remainingDistance: {agent.remainingDistance}");
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
                Debug.Log("AttackPlayer");
                float damage = stats.GetStat(StatType.Damage);
                playerCondition.TakeDamage(damage); // this reduces player's CurrentHealth
                Debug.Log($"{gameObject.name} attacked {player.name} for {damage} damage!");
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
            Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {stats.CurrentHealth}");
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
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }
}
