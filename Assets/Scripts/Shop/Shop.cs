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
    public Image itemBorder;

    private EquipmentScriptable rolledEquipment;

    void Start()
    {
        chestUI.SetActive(false);
        itemBorder.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
    }

    public void OnBuyChest()
    {
        if (GameManager.Instance.money >= chestCost)
        {
            GameManager.Instance.TakeMoney(chestCost);
            chestUI.SetActive(true);
        }
        else
        {
            Debug.Log("No Money");
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
        
        Debug.Log($"Got Equipment: {rolledEquipment.displayName} (+{rolledEquipment.boostValue} {rolledEquipment.value})");
    }
}
