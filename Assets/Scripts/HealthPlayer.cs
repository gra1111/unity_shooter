using TMPro;
using UnityEngine;

public class HealthPlayer : Health
{
    public TextMeshProUGUI healthText;

    public GameManager gameManager;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        healthText.text = Mathf.Clamp(CurrentHealth, 0, maxHealth).ToString();
    }
    protected override void Die()
    {
        base.Die();
        gameManager.GameOver();
    }
    public override void Heal(int amount)
    {
        base.Heal(amount);
        healthText.text = Mathf.Clamp(CurrentHealth, 0, maxHealth).ToString();
    }


}
