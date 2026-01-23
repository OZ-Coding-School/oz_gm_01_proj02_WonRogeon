using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.4f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public IEnumerator FadeOut()
    {
        if (fadeImage == null)
            yield break;

        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage == null)
            yield break;

        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        if (fadeImage == null)
            yield break;

        float t = 0f;
        Color baseColor = fadeImage.color;

        while (t < fadeDuration)
        {
            // ¾À ÀüÈ¯ Áß ÆÄ±« ´ëºñ
            if (fadeImage == null)
                yield break;

            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        if (fadeImage != null)
            fadeImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, to);
    }
}
