using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Reference")]
    public int chestCost = 100;
    public TextMeshProUGUI goldtext;
    public GameObject chestUI;
    public GameObject chestAnimation;
    public List<EquipmentScriptable> equipments;
    public Image itemIcon;
    public GameObject itemBorder;

    [Header("Item Description")]
    public TextMeshProUGUI itemDescription;
    private EquipmentScriptable rolledEquipment;
    [Header("ErrorMessage")]
    public GameObject errorMessage;
    public TextMeshProUGUI errorMessageText;
    public UIInventory uiInventory;

    void Awake()
    {
        errorMessage.SetActive(false);
        chestUI.SetActive(false);
        itemBorder.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
    }

    public void OnBuyChest()
    {
        if (GameManager.Instance.money >= chestCost)
        {
            GameManager.Instance.TakeMoney(chestCost);

            // Reset UI state
            chestUI.SetActive(true);
            chestAnimation.SetActive(true);
            itemBorder.gameObject.SetActive(false);
            itemIcon.gameObject.SetActive(false);

            rolledEquipment = null;
        }
        else
        {
            NoMoneyLog();
        }
    }

    public void OnChestExplode() //The actual part where it show the chest exploding and the item coming out. 
    {
        int randomIndex = Random.Range(0, equipments.Count);
        rolledEquipment = equipments[randomIndex];

        itemIcon.sprite = rolledEquipment.icon;
        chestAnimation.SetActive(false);
        itemBorder.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);
        itemDescription.text = $"{rolledEquipment.displayName}\n\n{rolledEquipment.description}\n{rolledEquipment.value}: {rolledEquipment.boostValue}";

        if (uiInventory != null)
        {
            uiInventory.AddItem(rolledEquipment);
        }
    }

    public void CollectItemButton()
    {
        chestUI.SetActive(false);
    }

    public void NoMoneyLog()
    {
        if (errorMessage != null)
        {
            errorMessage.SetActive(false);
        }
        errorMessage.SetActive(true);
        errorMessageText.text = $"Lacking Gold";
    }

    public void CloseNoMoneyLog()
    {
        errorMessage.SetActive(false);
    }
}
