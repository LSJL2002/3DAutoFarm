using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Reference")]
    public StatHandler playerStats;

    [Header("UI Texts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI attackspeedText;
    public TextMeshProUGUI damageText;

    void Start()
    {
        UpdateUI();
    }

    void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats.OnStatChanged += UpdateUI;
        }
    }
    private void UpdateUI()
    {
        if (playerStats == null) return;

        float maxHealth = playerStats.GetStat(StatType.Health);
        healthText.text = $"Health: {playerStats.CurrentHealth}/{maxHealth}";
        defenseText.text = $"Defense: {playerStats.GetStat(StatType.Defense)}";
        speedText.text = $"Speed: {playerStats.GetStat(StatType.Speed)}";
        attackspeedText.text = $"AtkSpd: {playerStats.GetStat(StatType.AttackSpeed)}";
        damageText.text = $"Damage: {playerStats.GetStat(StatType.Damage)}";
    }
}
