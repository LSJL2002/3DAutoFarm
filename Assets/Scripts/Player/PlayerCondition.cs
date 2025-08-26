using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    private StatHandler statHandler;

    Condition health { get { return uiCondition.health; } }

    private void Start()
    {
        statHandler = GetComponent<StatHandler>();

        if (uiCondition != null && health != null && statHandler != null)
        {
            float maxHP = statHandler.GetStat(StatType.Health);
            health.SetValues(maxHP, maxHP);
        }
    }

    private void Update()
    {
        if (health != null && health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (health != null)
            health.Add(amount);
    }

    public void TakeDamage(float amount)
    {
        if (health != null)
            health.Subtract(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}
