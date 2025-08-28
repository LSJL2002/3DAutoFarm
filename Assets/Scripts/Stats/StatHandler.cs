using UnityEngine;
using System.Collections.Generic;
using System;

public class StatHandler : MonoBehaviour
{
    [SerializeField] private ScriptableStats baseStats;

    private Dictionary<StatType, float> statValues = new Dictionary<StatType, float>();

    // True current health
    public float CurrentHealth { get; set; }

    public event Action OnStatChanged;

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
        OnStatChanged?.Invoke(); //When there is a stat change
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
        OnStatChanged?.Invoke();
    }

    public void RemoveModifier(StatType type, float amount) //When removing equipments
    {
        if (!statValues.ContainsKey(type)) return;
        statValues[type] -= amount;
        OnStatChanged.Invoke();
    }
}
