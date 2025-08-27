using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum EquipmentType
{
    Head,
    Body,
    Weapon
}

public enum Value
{
    Defense,
    Attack,
    Health
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class EquipmentScriptable : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public EquipmentType type;
    public Value value;
    public int boostValue;
    public Sprite icon;
}