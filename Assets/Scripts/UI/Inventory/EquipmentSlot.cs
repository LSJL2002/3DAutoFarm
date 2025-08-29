using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EquipmentType slotType;
    public EquipmentScriptable currentItem;
    public Image icon;

    private Image draggedIcon;

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
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (draggedSlot == null || draggedSlot.item == null) return;

        StatHandler stats = CharacterManager.Instance.Player.GetComponent<StatHandler>();
        if (stats == null) return;

        if (CanEquip(draggedSlot.item))
        {
            // If this slot already has an item, return it to inventory
            if (currentItem != null)
            {
                draggedSlot.inventory.AddItem(currentItem);
                Unequip(stats);
            }

            Equip(draggedSlot.item, stats);
            draggedSlot.Clear();
        }
    }

    // -------- DRAG FROM EQUIPMENT --------
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        icon.enabled = false;

        draggedIcon = new GameObject("DraggedEquipIcon", typeof(Image)).GetComponent<Image>();
        draggedIcon.transform.SetParent(transform.root);
        draggedIcon.transform.SetAsLastSibling();
        draggedIcon.sprite = icon.sprite;
        draggedIcon.rectTransform.sizeDelta = icon.rectTransform.sizeDelta;
        draggedIcon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedIcon == null) return;
        draggedIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedIcon != null) Destroy(draggedIcon.gameObject);

        // Check if dropped on inventory slot
        var inventorySlot = eventData.pointerCurrentRaycast.gameObject?.GetComponent<ItemSlot>();

        StatHandler stats = CharacterManager.Instance.Player.GetComponent<StatHandler>();

        if (inventorySlot != null && inventorySlot.item == null)
        {
            inventorySlot.SetItem(currentItem);
            Unequip(stats);
        }
        else
        {
            UpdateIcon();
        }
    }
}
