using System.Collections;
using TMPro;
using UnityEngine;

public class TextEffectsController : MonoBehaviour
{
    public TextMeshProUGUI[] textObjects; // Array to hold multiple text objects
    public float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(ApplyEffects());
    }

    IEnumerator ApplyEffects()
    {
        foreach (TextMeshProUGUI text in textObjects)
        {
            yield return StartCoroutine(FadeInText(text));
            yield return new WaitForSeconds(1f); // Optional delay before fading out
            yield return StartCoroutine(FadeOutText(text));
        }
    }

    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 0;
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        text.color = color;
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        text.color = color;
    }
}
