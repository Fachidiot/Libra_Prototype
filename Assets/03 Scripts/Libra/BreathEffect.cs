using System.Collections;
using TMPro;
using UnityEngine;

public class BreathEffect : MonoBehaviour
{
    [SerializeField] private float breathSpeed = 1.0f;
    [SerializeField, Range(0, 1)] private float minAlpha = 0.3f;
    [SerializeField, Range(0, 1)] private float maxAlpha = 1.0f;
    
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        if (text != null)
        {
            StartCoroutine(Breathing());
        }
        else
        {
            Debug.LogError("TMP_Text component not found on this GameObject.", this);
        }
    }

    IEnumerator Breathing()
    {
        while (true)
        {
            // Breath In
            yield return StartCoroutine(Fade(new Color(text.color.r, text.color.g, text.color.b, minAlpha), new Color(text.color.r, text.color.g, text.color.b, maxAlpha), 1f / breathSpeed));
            // Breath Out
            yield return StartCoroutine(Fade(new Color(text.color.r, text.color.g, text.color.b, maxAlpha), new Color(text.color.r, text.color.g, text.color.b, minAlpha), 1f / breathSpeed));
        }
    }

    IEnumerator Fade(Color start, Color end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            text.color = Color.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        text.color = end;
    }
}