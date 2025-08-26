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
                agent.isStopped = false;
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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 stopPosition = player.transform.position - direction * attackRange;

        if (Vector3.Distance(agent.destination, stopPosition) > 0.1f)
            agent.SetDestination(stopPosition);
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
                playerCondition.TakeDamage(damage);
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
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }
}
