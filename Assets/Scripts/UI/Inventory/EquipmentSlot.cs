using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public EquipmentType slotType;
    public EquipmentScriptable currentItem;
    public Image icon; // assign in inspector

    public bool CanEquip(EquipmentScriptable item)
    {
        return item != null && item.type == slotType;
    }

    public void Equip(EquipmentScriptable item, StatHandler stats)
    {
        if (currentItem != null)
            Unequip(stats);

        currentItem = item;
        ApplyStats(stats, item);

        UpdateIcon();
    }

    public void Unequip(StatHandler stats)
    {
        if (currentItem == null) return;

        RemoveStats(stats, currentItem);
        currentItem = null;

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (icon != null)
        {
            icon.sprite = currentItem != null ? currentItem.icon : null;
            icon.enabled = currentItem != null;
        }
    }

    private void ApplyStats(StatHandler stats, EquipmentScriptable item)
    {
        switch (item.value)
        {
            case Value.Attack:
                stats.AddModifier(StatType.Damage, item.boostValue);
                break;
            case Value.Defense:
                stats.AddModifier(StatType.Defense, item.boostValue);
                break;
            case Value.Health:
                stats.AddModifier(StatType.Health, item.boostValue);
                stats.CurrentHealth += item.boostValue;
                break;
        }
    }

    private void RemoveStats(StatHandler stats, EquipmentScriptable item)
    {
        switch (item.value)
        {
            case Value.Attack:
                stats.RemoveModifier(StatType.Damage, item.boostValue);
                break;
            case Value.Defense:
                stats.RemoveModifier(StatType.Defense, item.boostValue);
                break;
            case Value.Health:
                stats.RemoveModifier(StatType.Health, item.boostValue);
                stats.CurrentHealth = Mathf.Min(stats.CurrentHealth, stats.GetStat(StatType.Health));
                break;
        }
    }
}
