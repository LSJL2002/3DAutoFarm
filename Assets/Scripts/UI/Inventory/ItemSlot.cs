using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public EquipmentScriptable item;
    public Image icon;
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector3 originalPos;

    public void SetItem(EquipmentScriptable newItem)
    {
        item = newItem;
        icon.sprite = item != null ? item.icon : null;
        icon.enabled = item != null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        originalParent = transform.parent;
        originalPos = transform.localPosition;
        transform.SetParent(transform.root); // drag on top of UI
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.localPosition = originalPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (dragged == null || dragged == this) return;

        var equipSlot = GetComponent<EquipmentSlot>();
        if (equipSlot != null && equipSlot.CanEquip(dragged.item))
        {
            var playerStats = CharacterManager.Instance.Player.GetComponent<StatHandler>();
            equipSlot.Equip(dragged.item, playerStats);
            dragged.SetItem(null);
        }
    }
}
