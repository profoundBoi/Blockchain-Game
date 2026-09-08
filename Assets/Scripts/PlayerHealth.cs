using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;

        currentHealth -= damage;

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        PlayerRespawn respawn = GetComponent<PlayerRespawn>();

        if (respawn != null)
        {
            respawn.Respawn();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}