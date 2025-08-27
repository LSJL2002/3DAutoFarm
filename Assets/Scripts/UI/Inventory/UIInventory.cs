using UnityEngine;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
    [Header("UI References")]
    public List<ItemSlot> itemSlots; // assign your 10 pre-placed slots in the inspector

    void Start()
    {
        // Initialize each slot
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].inventory = this;
            itemSlots[i].index = i;
            itemSlots[i].Clear();
        }
    }

    public void AddItem(EquipmentScriptable newItem)
    {
        // Find first empty slot
        ItemSlot empty = itemSlots.Find(s => s.item == null);
        if (empty != null)
        {
            empty.SetItem(newItem);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (var slot in itemSlots)
        {
            slot.icon.enabled = slot.item != null;
        }
    }
}
