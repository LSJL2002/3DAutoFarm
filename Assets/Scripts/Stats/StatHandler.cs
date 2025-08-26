using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}
public class StatHandler : MonoBehaviour, IDamageable
{
    [SerializeField] private ScriptableStats baseStats;

    private Dictionary<StatType, float> statValues = new Dictionary<StatType, float>();
    private float currentHealth;

    void Awake()
    {
        foreach (var entry in baseStats.stats)
        {
            statValues[entry.StatType] = entry.baseValue;
        }
        currentHealth = GetStat(StatType.Health);
    }

    public float GetStat(StatType type)
    {
        return statValues.ContainsKey(type) ? statValues[type] : 0f;
    }

    public void TakeDamage(float damage)
    {
        float defense = GetStat(StatType.Defense);
        float finalDamage = Mathf.Max(0, damage - defense);
        currentHealth -= finalDamage;

        Debug.Log($"{gameObject.name} took {finalDamage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }
}
