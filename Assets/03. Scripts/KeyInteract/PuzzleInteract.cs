using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteract : MonoBehaviour, IInteractable
{
    [Header("Puzzle UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private CanvasGroup puzzleCanvasGroup;

    [Header("Screen Fade")]
    [SerializeField] private CanvasGroup screenFadeGroup;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.8f;

    [Header("Monologue SFX")]
    [SerializeField] private AudioClip puzzleClearMonologueSFX;

    [Header("Puzzle Clear SFX")]
    [SerializeField] private AudioClip wallBreakSFX;
    [SerializeField] private float wallBreakDelay = 0.3f;


    private bool isOpened;
    private bool isSolved;

    private void OnEnable()
    {
        PuzzleAnswerChecker.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDisable()
    {
        PuzzleAnswerChecker.OnPuzzleSolved -= HandlePuzzleSolved;
    }

    private void Update()
    {
        if (!isOpened || isSolved)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzleInternal();
        }
    }

    public void Interact()
    {
        if (isOpened || isSolved)
            return;

        OpenPuzzle();
    }

    private void OpenPuzzle()
    {
        isOpened = true;

        puzzlePanel.SetActive(true);
        puzzleCanvasGroup.alpha = 1f;
        puzzleCanvasGroup.blocksRaycasts = true;

        Time.timeScale = 0f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = false;
    }

    private void HandlePuzzleSolved()
    {
        if (isSolved) return;

        isSolved = true;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(PuzzleClearSequence());
    }

    private IEnumerator PuzzleClearSequence()
    {
        // 1. 화면 어두워짐
        yield return StartCoroutine(
            Fade(screenFadeGroup, 0f, 1f, fadeOutDuration)
        );

        // 2. 퍼즐 패널 닫기
        ClosePuzzleInternal();

        // 3. 화면 밝아짐
        yield return StartCoroutine(
            Fade(screenFadeGroup, 1f, 0f, fadeInDuration)
        );

        // 3.5 아주 짧은 정적 (긴장감)
        yield return new WaitForSecondsRealtime(0.2f);

        // 4. 벽 부서지는 소리
        if (SoundManager.Instance != null && wallBreakSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Player.Instance.transform.position,
                wallBreakSFX
            );
        }

        // 5. 소리 인지할 시간
        yield return new WaitForSecondsRealtime(wallBreakDelay);

        // 6. 아야 독백 시작 + 첫 독백 SFX
        if (SoundManager.Instance != null && puzzleClearMonologueSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Player.Instance.transform.position,
                puzzleClearMonologueSFX
            );
        }

        if (MonologueUI.Instance != null)
        {
            MonologueUI.Instance.ShowSequence(
                new List<MonologueLine>
                {
                    new MonologueLine
                    {
                        character = "Aya",
                        expression = "Frightened",
                        message = "어디서 벽 부서지는 소리가 났어!"
                    },
                    new MonologueLine
                    {
                        character = "Aya",
                        expression = "Smile",
                        message = "확인해볼까?"
                    }
                }
            );
        }
    }

    private void ClosePuzzleInternal()
    {
        isOpened = false;

        puzzleCanvasGroup.blocksRaycasts = false;
        puzzlePanel.SetActive(false);

        Time.timeScale = 1f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;
    }

    private IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0f;
        group.alpha = from;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        group.alpha = to;
    }
}
