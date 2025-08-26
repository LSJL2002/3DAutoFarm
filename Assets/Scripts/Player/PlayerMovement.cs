using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private StatHandler stats;

    [SerializeField] private float attackRange = 2f;
    private float lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<StatHandler>();

        // Use movement speed from stats
        agent.speed = stats.GetStat(StatType.Speed);
    }

    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);

            if (distance > attackRange)
            {
                agent.SetDestination(nearestEnemy.transform.position);
            }
            else
            {
                agent.ResetPath();

                float attackSpeed = stats.GetStat(StatType.AttackSpeed);
                float attackCooldown = 1f / attackSpeed; // attacks per second → time between attacks

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack(nearestEnemy);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void Attack(GameObject enemy)
    {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float damage = stats.GetStat(StatType.Damage);
            damageable.TakeDamage(damage);
            Debug.Log($"{gameObject.name} attacked {enemy.name} for {damage} damage!");
        }
    }

    GameObject FindNearestEnemy()
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
