using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Colors")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;

    [Header("Numbers")]
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private float fullColorDuration = 0.1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #region Damage Effect
    public void DamageEffect()
    {
        StartCoroutine(DamageEffectRoutine(damageColor, transitionTime, fullColorDuration));
    }

    private IEnumerator DamageEffectRoutine(Color newColor, float transitionTime, float fullColorDuration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = newColor;

        yield return new WaitForSeconds(fullColorDuration);

        //Lerp back to initial color
        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(newColor, originalColor, elapsed / transitionTime);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    #endregion

    #region Heal Effect

    public void HealEffect()
    {
        StartCoroutine(HealEffectRoutine(healColor, transitionTime, fullColorDuration));
    }

    private IEnumerator HealEffectRoutine(Color newColor, float transitionTime, float fullColorDuration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = newColor;

        yield return new WaitForSeconds(fullColorDuration);

        //Lerp back to initial color
        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(newColor, originalColor, elapsed / transitionTime);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    #endregion
}
