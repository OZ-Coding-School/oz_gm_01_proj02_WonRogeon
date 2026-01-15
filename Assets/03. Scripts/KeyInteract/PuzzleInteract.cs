using UnityEngine;
using System.Collections;

public class PuzzleInteract : MonoBehaviour, IInteractable
{
    [Header("Puzzle UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private CanvasGroup puzzleCanvasGroup;

    [Header("Screen Fade")]
    [SerializeField] private CanvasGroup screenFadeGroup;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.8f;

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

    // 닫는건 따로 포함안되어있어서 업데이트에서 처리하자
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
        StartCoroutine(PuzzleClearSequence());
    }

    private IEnumerator PuzzleClearSequence()
    {
        // 1. 화면 점점 어두워짐
        yield return StartCoroutine(Fade(screenFadeGroup, 0f, 1f, fadeOutDuration));

        // 2. 완전히 어두워졌을 때 퍼즐 패널 닫기
        ClosePuzzleInternal();

        // 3. 화면 점점 밝아짐
        yield return StartCoroutine(Fade(screenFadeGroup, 1f, 0f, fadeInDuration));
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
