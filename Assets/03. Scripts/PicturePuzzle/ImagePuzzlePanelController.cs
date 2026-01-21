using UnityEngine;
using System.Collections;

public class ImagePuzzlePanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private ImagePuzzleInteract trigger;

    public void OnPuzzleSolved()
    {
        StartCoroutine(ClearSequence());
    }

    private IEnumerator ClearSequence()
    {
        // 1. 페이드 아웃 (unscaled)
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. 퍼즐 패널 닫기
        puzzlePanel.SetActive(false);

        // 3. 퍼즐 재진입 방지
        if (trigger != null)
            trigger.gameObject.SetActive(false);


        // 4. 게임 상태 복구
        Time.timeScale = 1f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;

        // 5. 페이드 인
        yield return StartCoroutine(Fade(1f, 0f));
    }


    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        const float duration = 0.6f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        fadeGroup.alpha = to;
    }
}
