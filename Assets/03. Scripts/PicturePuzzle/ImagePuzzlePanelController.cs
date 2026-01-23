using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePuzzlePanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private ImagePuzzleInteract trigger;

    [Header("Clear Result")]
    [SerializeField] private GameObject[] disableTargets;

    [Header("Clear SFX")]
    [SerializeField] private AudioClip doorOpenSFX;
    [SerializeField] private AudioClip ayaMonologueSFX;
    [SerializeField] private float doorOpenDelay = 0.4f;

    public void OnPuzzleSolved()
    {
        StartCoroutine(ClearSequence());
    }

    private IEnumerator ClearSequence()
    {
        // 1. 페이드 아웃
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. 퍼즐 패널 닫기
        puzzlePanel.SetActive(false);

        // 3. 퍼즐 클리어 결과 반영
        if (disableTargets != null)
        {
            foreach (var obj in disableTargets)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        // 4. 퍼즐 재진입 방지
        if (trigger != null)
            trigger.gameObject.SetActive(false);

        // 5. 게임 상태 복구
        Time.timeScale = 1f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;

        // 6. 페이드 인
        yield return StartCoroutine(Fade(1f, 0f));

        // =========================
        // 7. 문 열리는 소리
        // =========================
        if (SoundManager.Instance != null && doorOpenSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Player.Instance.transform.position,
                doorOpenSFX
            );
        }

        // 소리 여운
        yield return new WaitForSecondsRealtime(doorOpenDelay);

        // =========================
        // 8. 메시지 UI
        // =========================
        bool messageClosed = false;

        MessageUI.Instance.Show(
            "잠겨있던 기계문이 열렸다.",
            () => messageClosed = true
        );

        yield return new WaitUntil(() => messageClosed);

        // =========================
        // 9. 짧은 정적
        // =========================
        yield return new WaitForSecondsRealtime(0.2f);

        // =========================
        // 10. 아야 독백 + 숨소리
        // =========================
        if (SoundManager.Instance != null && ayaMonologueSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Player.Instance.transform.position,
                ayaMonologueSFX
            );
        }

        var monologueLines = new List<MonologueLine>
    {
        new MonologueLine
        {
            character = "Aya",
            expression = "Default",
            message = "문이 열렸나봐"
        },
        new MonologueLine
        {
            character = "Aya",
            expression = "Smile",
            message = "엄마랑 아빠도 저기에 계실까?"
        }
    };

        MonologueUI.Instance.ShowSequence(monologueLines);
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
