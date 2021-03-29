using UnityEngine;

/*
 * This script is a generic health component for
 * any item that needs to have health.  This can
 * be added to the player, enemies. It can also be extended by
 * inheriting from it for specific interactions desired.
 */

public class Health : MonoBehaviour
{
    [Tooltip("Max and current health")]
    [Header("Health values")]
    [SerializeField] private int maximumHealth;
    [SerializeField] private int currentHealth;

    public void SetHealth(int amount)
    {
        currentHealth = amount;
    }

    public virtual void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
    }

    public void Kill()
    {
        currentHealth = 0;
    }

    public void FullHeal()
    {
        currentHealth = maximumHealth;
    }
}
