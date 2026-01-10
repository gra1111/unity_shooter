using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }

    public event Action<Health> OnDeath;
    public event Action<Health, int> OnDamaged;

    [SerializeField] private int minHeal = 1;
    [SerializeField] private int maxHeal = 5;

    public Image healthBar;
    [SerializeField] private int scorePoints;
    void Awake()
    {
        CurrentHealth = maxHealth;
    
    }
    void Start()
    {
        healthBar.fillAmount = 1;
    }

    protected virtual void Update()
    {
        //if the object falls below y = -5, it dies
        if (transform.position.y < -5 && CurrentHealth > 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0)
        { return; }

        CurrentHealth -= amount;
        OnDamaged?.Invoke(this, amount);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);

        // Heal the player when this enemy dies, if this is not the player itself
        if (GetComponent<HealthPlayer>() == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null && playerHealth.CurrentHealth > 0)
                {
                    int healAmount = UnityEngine.Random.Range(minHeal, maxHeal+1); // 1..5
                    playerHealth.Heal(healAmount);
                }
            }
        }

        if (ScoreManager._instance != null)
        {
            ScoreManager._instance.AddScore(scorePoints); 
        }
    }

    public virtual void Heal(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {

        healthBar.fillAmount = (float)CurrentHealth / maxHealth;

    }

}
