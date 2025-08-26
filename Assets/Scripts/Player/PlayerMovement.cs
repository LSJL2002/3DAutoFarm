using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Run,
    Attacking
}

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private StatHandler stats;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private float lastAttackTime = 0f;
    private AIState currentState = AIState.Run;

    private GameObject targetEnemy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<StatHandler>();

        if (stats != null)
            agent.speed = stats.GetStat(StatType.Speed);
    }

    void Update()
    {
        targetEnemy = FindNearestEnemy();
        if (targetEnemy == null) return;

        float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
        currentState = distance > attackRange ? AIState.Run : AIState.Attacking;

        switch (currentState)
        {
            case AIState.Run:
                HandleRunState();
                break;
            case AIState.Attacking:
                HandleAttackState();
                break;
        }
    }

    private void HandleRunState()
    {
        Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
        Vector3 stopPosition = targetEnemy.transform.position - direction * attackRange;

        agent.isStopped = false;
        agent.SetDestination(stopPosition);
    }

    private void HandleAttackState()
    {
        agent.isStopped = true;

        RotateTowards(targetEnemy.transform.position);

        float attackSpeed = stats.GetStat(StatType.AttackSpeed);
        float attackCooldown = attackSpeed > 0 ? 1f / attackSpeed : 1f;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack(targetEnemy);
            lastAttackTime = Time.time;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        if (direction.magnitude == 0f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void Attack(GameObject enemy)
    {
        if (enemy == null) return;

        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float damage = stats.GetStat(StatType.Damage);
            damageable.TakeDamage(damage);
            Debug.Log($"{gameObject.name} attacked {enemy.name} for {damage} damage!");
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
