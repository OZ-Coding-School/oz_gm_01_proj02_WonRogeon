using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClearSequenceController : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeGroup;

    [Header("Clear Text")]
    [SerializeField] private TMP_Text clearText;

    [Header("End Menu UI")]
    [SerializeField] private RectTransform retryText;
    [SerializeField] private RectTransform quitText;
    [SerializeField] private RectTransform selectionBar;

    [Header("Tween Settings")]
    [SerializeField] private float menuMoveDistance = 60f;
    [SerializeField] private float menuMoveDuration = 0.6f;
    [SerializeField] private float menuInterval = 0.15f;
    [SerializeField] private float selectionMoveDuration = 0.15f;

    [Header("Scene")]
    [SerializeField] private string mainSceneName = "MainScene";

    [Header("UI SFX")]
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] private AudioClip confirmSFX;

    [Header("BGM")]
    [SerializeField] private AudioClip clearBGM;

    private bool canInput;
    private int currentIndex; // 0 = Retry, 1 = Quit
    private Tween selectionTween;

    private void Start()
    {
        InitUI();

        if (SoundManager.Instance != null && clearBGM != null)
            SoundManager.Instance.PlayBGM(clearBGM);

        StartSequence();
    }

    private void InitUI()
    {
        fadeGroup.alpha = 1f;

        clearText.alpha = 0f;
        clearText.gameObject.SetActive(true);

        retryText.gameObject.SetActive(false);
        quitText.gameObject.SetActive(false);
        selectionBar.gameObject.SetActive(false);

        canInput = false;
    }

    private void StartSequence()
    {
        // 1. 페이드 인
        fadeGroup.DOFade(0f, 1.2f)
            .OnComplete(ShowClearText);
    }

    private void ShowClearText()
    {
        clearText
            .DOFade(1f, 1.0f)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.6f, ShowEndMenu);
            });
    }

    private void ShowEndMenu()
    {
        Vector2 retryOrigin = retryText.anchoredPosition;
        Vector2 quitOrigin = quitText.anchoredPosition;

        retryText.anchoredPosition = retryOrigin - Vector2.up * menuMoveDistance;
        quitText.anchoredPosition = quitOrigin - Vector2.up * menuMoveDistance;

        retryText.gameObject.SetActive(true);
        quitText.gameObject.SetActive(true);

        retryText
            .DOAnchorPos(retryOrigin, menuMoveDuration)
            .SetEase(Ease.OutBack);

        DOVirtual.DelayedCall(menuInterval, () =>
        {
            quitText
                .DOAnchorPos(quitOrigin, menuMoveDuration)
                .SetEase(Ease.OutBack);
        });

        DOVirtual.DelayedCall(menuMoveDuration + menuInterval, () =>
        {
            selectionBar.gameObject.SetActive(true);
            currentIndex = 0;
            UpdateSelectionImmediate();
            canInput = true;
        });
    }

    private void Update()
    {
        if (!canInput)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentIndex != 0)
            {
                currentIndex = 0;

                if (SoundManager.Instance != null && moveSFX != null)
                    SoundManager.Instance.PlayUISFX(moveSFX);

                UpdateSelection();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentIndex != 1)
            {
                currentIndex = 1;

                if (SoundManager.Instance != null && moveSFX != null)
                    SoundManager.Instance.PlayUISFX(moveSFX);

                UpdateSelection();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundManager.Instance != null && confirmSFX != null)
                SoundManager.Instance.PlayUISFX(confirmSFX);

            ExecuteSelection();
        }
    }

    private void UpdateSelection()
    {
        RectTransform target = currentIndex == 0 ? retryText : quitText;

        selectionTween?.Kill();
        selectionTween = selectionBar
            .DOMove(target.position, selectionMoveDuration)
            .SetEase(Ease.OutQuad);

        UpdateTextColor();
    }

    private void UpdateSelectionImmediate()
    {
        selectionBar.position = retryText.position;
        UpdateTextColor();
    }

    private void UpdateTextColor()
    {
        retryText.GetComponent<TMP_Text>().color =
            currentIndex == 0 ? Color.black : Color.white;

        quitText.GetComponent<TMP_Text>().color =
            currentIndex == 1 ? Color.black : Color.white;
    }

    private void ExecuteSelection()
    {
        if (!canInput)
            return;

        canInput = false;

        if (currentIndex == 0)
        {
            StartCoroutine(RetryRoutine());
        }
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator RetryRoutine()
    {
        // BGM 페이드 아웃
        if (SoundManager.Instance != null)
            yield return SoundManager.Instance.FadeOutAndStopBGM(this);

        // 화면 페이드 아웃
        yield return FadeController.Instance.FadeOut();

        SceneManager.LoadScene(mainSceneName);
    }


}
