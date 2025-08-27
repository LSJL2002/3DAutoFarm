using UnityEngine;
using System.Collections.Generic;

public class StatHandler : MonoBehaviour
{
    [SerializeField] private ScriptableStats baseStats;

    private Dictionary<StatType, float> statValues = new Dictionary<StatType, float>();

    // True current health
    public float CurrentHealth { get; set; }

    void Awake()
    {
        foreach (var entry in baseStats.stats)
            statValues[entry.StatType] = entry.baseValue;

        CurrentHealth = GetStat(StatType.Health);
    }

    public float GetStat(StatType type)
    {
        return statValues.ContainsKey(type) ? statValues[type] : 0f;
    }

    public void TakeDamage(float damage)
    {
        float defense = GetStat(StatType.Defense);
        float finalDamage = Mathf.Max(0f, damage - defense);

        CurrentHealth -= finalDamage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f); // Clamp to zero
        Debug.Log($"{gameObject.name} took {finalDamage} damage! Remaining HP: {CurrentHealth}");
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, GetStat(StatType.Health)); // Clamp to max
    }
    public void AddModifier(StatType type, float amount) //For Equipments
    {
        if (!statValues.ContainsKey(type)) statValues[type] = 0;
        statValues[type] += amount;
    }

    public void RemoveModifier(StatType type, float amount) //When removing equipments
    {
        if (!statValues.ContainsKey(type)) return;
        statValues[type] -= amount;
    }
}
