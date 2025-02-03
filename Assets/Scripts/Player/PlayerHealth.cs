using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //Player Health
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    //Player Shield
    public GameObject playerShield;
    [SerializeField] private bool hasShield = false;

    public Slider energyBar;

    //Sprite Colors
    private SpriteRenderer spriteRenderer;
    private Color baseColor = Color.white;
    private Color damageColor = Color.red;
    private Color healColor = Color.green;

    //Events
    public delegate void PlayerDiedHandler();
    public event PlayerDiedHandler OnPlayerDied;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer not found on Player!");
        }
    }
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

        StartCoroutine(ColorFlashCoroutine(baseColor, damageColor, 0.5f, 0.1f));  //Damage Effect

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
        StartCoroutine(ColorFlashCoroutine(baseColor, healColor, 0.5f, 0.3f));  //Heal Effect

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

    private IEnumerator ColorFlashCoroutine(Color initialColor, Color newColor, float transitionTime, float fullColorDuration)
    {
        spriteRenderer.color = newColor;

        yield return new WaitForSeconds(fullColorDuration);

        //Lerp back to initial color
        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(newColor, initialColor, elapsed / transitionTime);
            yield return null;
        }

        spriteRenderer.color = initialColor;
    }
}