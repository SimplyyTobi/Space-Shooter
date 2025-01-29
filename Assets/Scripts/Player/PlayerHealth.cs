using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public GameObject playerShield;
    [SerializeField] private bool hasShield = false;

    public Slider energyBar;

    #region Events
    public delegate void PlayerDiedHandler();
    public event PlayerDiedHandler OnPlayerDied;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateEnergyBar();
    }

    public void TakeDamage(int amount)
    {
        if (hasShield)
        {
            DeactivateShield();
            return;
        }
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateEnergyBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = currentHealth;
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
    }

    public void ActivateShield()
    {
        hasShield = true;
        playerShield.SetActive(true);
    }

    private void DeactivateShield()
    {
        hasShield = false;
        playerShield.SetActive(false);
    }
}