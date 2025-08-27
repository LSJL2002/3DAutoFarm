using UnityEngine;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
    public Transform inventoryGrid;
    public GameObject itemSlotPrefab;
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    public void AddItem(EquipmentScriptable item)
    {
        ItemSlot slot = itemSlots.Find(s => s.item == null);
        if (slot == null)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, inventoryGrid);
            slot = newSlot.GetComponent<ItemSlot>();
            slot.inventory = this;
            itemSlots.Add(slot);
        }

        slot.SetItem(item);
    }

    public void SelectItem(ItemSlot slot)
    {
        // Optional: highlight selected item, show description, etc.
    }
}
