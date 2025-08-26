using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    private StatHandler statHandler;

    private Condition health => uiCondition.health;

    void Start()
    {
        statHandler = GetComponent<StatHandler>();
        if (statHandler != null && health != null)
        {
            // Initialize UI
            health.SetValues(statHandler.CurrentHealth, statHandler.GetStat(StatType.Health));
        }
    }

    void Update()
    {
        if (statHandler == null || health == null) return;

        // Update UI
        health.curValue = statHandler.CurrentHealth;

        // Die if health is 0
        if (statHandler.CurrentHealth <= 0f)
            Die();
    }

    public void TakeDamage(float amount)
    {
        if (statHandler != null)
            statHandler.TakeDamage(amount);
    }

    public void Heal(float amount)
    {
        if (statHandler != null)
            statHandler.Heal(amount);
    }

    private void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
}

