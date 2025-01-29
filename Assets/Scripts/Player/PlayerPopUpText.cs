using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPopUpText : MonoBehaviour
{
    private TextMeshPro popUpText;
    public GameObject popUpTextObject;
    [SerializeField] private float displayDuration = 1.5f;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (popUpTextObject == null)
        {
            Debug.LogError("PopUpText (GameObject) not found on the player!");
        }

        popUpText = popUpTextObject.GetComponent<TextMeshPro>();
        if (popUpText == null)
        {
            Debug.LogError("TextMeshPro component not found on PopUpText (GameObject)!");
        }
    }

    public void DisplayMessage(string message)
    {
        popUpText.text = message;
        popUpText.color = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, 1f);
        popUpText.gameObject.SetActive(true);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(HideAfterDelayCoroutine());
    }

    private IEnumerator HideAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(displayDuration - fadeDuration);

        float time = 0f;
        Color initialColor = popUpText.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            popUpText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        popUpText.gameObject.SetActive(false);
        currentCoroutine = null;
    }
}
