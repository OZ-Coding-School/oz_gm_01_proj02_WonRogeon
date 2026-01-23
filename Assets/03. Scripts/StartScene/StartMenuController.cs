using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text[] menuTexts;
    [SerializeField] private RectTransform selectionBar;
    [SerializeField] private GameObject creditsPanel;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.black;

    [Header("Scene")]
    [SerializeField] private string mainSceneName = "MainScene";

    [Header("Tween")]
    [SerializeField] private float selectionMoveDuration = 0.15f;

    [Header("BGM")]
    [SerializeField] private AudioClip startBGM;

    [Header("UI SFX")]
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] private AudioClip confirmSFX;
    [SerializeField] private AudioClip backSFX;

    [Header("Option")]
    [SerializeField] private GameObject optionPanel;


    private bool isTransitioning = false;
    private bool isCreditsOpen = false;

    private int currentIndex = 0;
    private float inputCooldown = 0.1f;
    private float lastInputTime;

    private Tween selectionTween;

    private void Start()
    {
        UpdateVisualImmediate();

        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (SoundManager.Instance != null && startBGM != null)
            SoundManager.Instance.PlayBGM(startBGM);
    }

    private void Update()
    {
        if (isTransitioning)
            return;

        if (isCreditsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayUISFX(backSFX);
                CloseCredits();
            }
            return;
        }

        HandleMoveInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayUISFX(confirmSFX);
            HandleSubmitInput();
        }
    }

    private void HandleMoveInput()
    {
        if (Time.time - lastInputTime < inputCooldown)
            return;

        int dir = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            dir = -1;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            dir = 1;

        if (dir == 0)
            return;

        currentIndex += dir;

        if (currentIndex < 0)
            currentIndex = menuTexts.Length - 1;
        else if (currentIndex >= menuTexts.Length)
            currentIndex = 0;

        lastInputTime = Time.time;

        PlayUISFX(moveSFX);
        UpdateVisual();
    }

    private void HandleSubmitInput()
    {
        switch (currentIndex)
        {
            case 0:
                StartCoroutine(StartGameRoutine());
                break;

            case 2: // Option
                OpenOption();
                break;
            case 3:
                OpenCredits();
                break;

            case 4:
                Application.Quit();
                break;
        }
    }

    private IEnumerator StartGameRoutine()
    {
        isTransitioning = true;

        if (SoundManager.Instance != null)
            yield return SoundManager.Instance.FadeOutAndStopBGM(this);

        yield return FadeController.Instance.FadeOut();
        SceneManager.LoadScene(mainSceneName);
    }

    private void OpenCredits()
    {
        if (creditsPanel == null)
            return;

        creditsPanel.SetActive(true);
        isCreditsOpen = true;
    }

    private void CloseCredits()
    {
        creditsPanel.SetActive(false);
        isCreditsOpen = false;
    }

    private void OpenOption()
    {
        if (optionPanel == null)
            return;

        PlayUISFX(confirmSFX);

        optionPanel.SetActive(true);

        // 스타트 메뉴 입력 차단
        isTransitioning = true;
    }

    public void OnOptionClosed()
    {
        PlayUISFX(backSFX);
        isTransitioning = false;
    }




    private void UpdateVisual()
    {
        UpdateTextColor();

        Vector3 targetPos = new Vector3(
            selectionBar.position.x,
            menuTexts[currentIndex].transform.position.y,
            selectionBar.position.z
        );

        selectionTween?.Kill();

        selectionTween = selectionBar
            .DOMove(targetPos, selectionMoveDuration)
            .SetEase(Ease.OutQuad);
    }

    private void UpdateVisualImmediate()
    {
        UpdateTextColor();

        selectionBar.position = new Vector3(
            selectionBar.position.x,
            menuTexts[currentIndex].transform.position.y,
            selectionBar.position.z
        );
    }

    private void UpdateTextColor()
    {
        for (int i = 0; i < menuTexts.Length; i++)
        {
            menuTexts[i].color =
                (i == currentIndex) ? selectedColor : normalColor;
        }
    }

    private void PlayUISFX(AudioClip clip)
    {
        if (clip == null || SoundManager.Instance == null)
            return;

        SoundManager.Instance.PlayUISFX(clip);
    }
}
