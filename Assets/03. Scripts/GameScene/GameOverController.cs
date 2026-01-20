using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    public static GameOverController Instance;

    [Header("Fade UI")]
    [SerializeField] private CanvasGroup fadeGroup;

    [Header("Timing")]
    [SerializeField] private float freezeTime = 0.15f;
    [SerializeField] private float fadeDuration = 1.2f;

    [Header("Scene")]
    [SerializeField] private string endSceneName = "GameOverScene";

    private bool isGameOver;

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

    public void TriggerGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 1. 충돌 순간 정지
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);

        // 2. 화면 페이드 아웃
        yield return FadeOut();

        // 3. 타임스케일 복구 
        Time.timeScale = 1f;

        // 4. 종료씬 전환
        SceneManager.LoadScene(endSceneName);
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        fadeGroup.alpha = 0f;
        fadeGroup.gameObject.SetActive(true);

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = 1f;
    }
}
