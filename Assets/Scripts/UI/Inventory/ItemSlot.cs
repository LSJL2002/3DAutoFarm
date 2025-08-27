using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public EquipmentScriptable item;
    public Image icon;

    [HideInInspector] public UIInventory inventory;
    [HideInInspector] public int index;

    private Image draggedIcon;

    public void SetItem(EquipmentScriptable newItem)
    {
        item = newItem;
        icon.sprite = item != null ? item.icon : null;
        icon.enabled = item != null;
    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        icon.enabled = false;

        draggedIcon = new GameObject("DraggedIcon", typeof(Image)).GetComponent<Image>();
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
        icon.enabled = item != null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Drop on another inventory slot
        var draggedSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (draggedSlot != null && draggedSlot != this)
        {
            EquipmentScriptable temp = item;
            SetItem(draggedSlot.item);
            draggedSlot.SetItem(temp);
            inventory.UpdateUI();
            return;
        }

        // Drop on an equipment slot
        var equipSlot = eventData.pointerDrag?.GetComponent<EquipmentSlot>();
        if (equipSlot != null)
        {
            TryEquipToSlot(equipSlot);
        }
    }

    private void TryEquipToSlot(EquipmentSlot equipSlot)
    {
        if (item == null) return;

        StatHandler stats = CharacterManager.Instance.Player.GetComponent<StatHandler>();
        if (stats == null) return;

        if (equipSlot.CanEquip(item))
        {
            // Unequip current item if exists
            if (equipSlot.currentItem != null)
            {
                SetItem(equipSlot.currentItem); // return to inventory
            }
            else
            {
                Clear(); // remove item from slot
            }

            equipSlot.Equip(item, stats);

            inventory.UpdateUI();
        }
    }
}
