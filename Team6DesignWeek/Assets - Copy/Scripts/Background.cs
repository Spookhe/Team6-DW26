using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Camera mainCamera;

    public Color whiteColor = Color.white;
    public Color grayColor = new Color(0.2f, 0.2f, 0.2f);
    public float fadeDuration = 3f;

    private int lastMilestone = 0;
    private bool isGray = false;

    private void Start()
    {
        mainCamera.backgroundColor = whiteColor;
    }

    private void Update()
    {
        // Changes the game background colour every 250 points
        int milestone = Mathf.FloorToInt(GameManager.Instance.Score / 250);

        if (milestone != lastMilestone)
        {
            isGray = milestone % 2 == 1;
            Color target = isGray ? grayColor : whiteColor;

            StopAllCoroutines();
            StartCoroutine(FadeBackground(target));
            lastMilestone = milestone;
        }
    }

    // Fades the background colour transition (opposed to instant colour change)
    private IEnumerator FadeBackground(Color targetColor)
    {
        Color start = mainCamera.backgroundColor;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            mainCamera.backgroundColor = Color.Lerp(start, targetColor, elapsed / fadeDuration);
            yield return null;
        }

        mainCamera.backgroundColor = targetColor;
    }

    // Pulses background colour green (for UFO event)
    public IEnumerator PulseColor(Color pulseColor, float duration)
    {
        Color original = mainCamera.backgroundColor;
        float timer = 0f;

        while (timer < duration)
        {
            float pulse = Mathf.PingPong(timer * 2f, 1f);
            mainCamera.backgroundColor = Color.Lerp(original, pulseColor, pulse);
            timer += Time.deltaTime;
            yield return null;
        }

        mainCamera.backgroundColor = original;
    }


    // Revert back to original white background on game restart
    public void ResetBackground()
    {
        StopAllCoroutines();
        mainCamera.backgroundColor = whiteColor;
        lastMilestone = 0;
        isGray = false;
    }
}
