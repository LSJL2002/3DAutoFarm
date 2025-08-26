using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatType
{
    Health,
    Defense,
    Speed,
    AttackSpeed,
    Damage
}
[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/Character Stats")]
public class ScriptableStats : ScriptableObject
{
    public string characterName;
    public List<StatEntry> stats;
}

[System.Serializable]
public class StatEntry
{
    public StatType StatType;
    public float baseValue;
}
