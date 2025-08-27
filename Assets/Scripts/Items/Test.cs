using UnityEngine;

public class Test : MonoBehaviour
{
    public UIInventory inventory;      // Drag your UIInventory object here
    public EquipmentScriptable testItem; // Drag the Sword (or any item) here

    void Update()
    {
        // Press I to add the test item to the inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.AddItem(testItem);
            Debug.Log("Added Item");
        }
    }
}