using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverSequenceController : MonoBehaviour
{
    public static GameOverSequenceController Instance;

    [Header("References")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject ayaDeath;
    [SerializeField] private TMP_Text gameOverText;

    [Header("End Menu UI")]
    [SerializeField] private RectTransform retryText;
    [SerializeField] private RectTransform quitText;
    [SerializeField] private RectTransform selectionBar;

    [Header("Tween Settings")]
    [SerializeField] private float menuMoveDistance = 60f;
    [SerializeField] private float menuMoveDuration = 0.6f;
    [SerializeField] private float menuInterval = 0.15f;
    [SerializeField] private float selectionMoveDuration = 0.15f;

    private Animator bloodAnimator;
    private Animator ayaAnimator;
    private SpriteRenderer bloodSR;
    private SpriteRenderer ayaSR;

    // ===== 메뉴 제어 상태 =====
    private bool canInput;
    private int currentIndex; // 0 = Retry, 1 = Quit

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bloodAnimator = bloodEffect.GetComponent<Animator>();
        ayaAnimator = ayaDeath.GetComponent<Animator>();

        bloodSR = bloodEffect.GetComponent<SpriteRenderer>();
        ayaSR = ayaDeath.GetComponent<SpriteRenderer>();

        bloodEffect.SetActive(false);
        ayaDeath.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        retryText.gameObject.SetActive(false);
        quitText.gameObject.SetActive(false);
        selectionBar.gameObject.SetActive(false);

        canInput = false;

        PlayBloodEffect();
    }

    private void Update()
    {
        if (!canInput)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = 1 - currentIndex;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteSelection();
        }
    }

    private void PlayBloodEffect()
    {
        bloodEffect.SetActive(true);
        bloodSR.color = new Color(1, 1, 1, 1);
        bloodAnimator.Play(0, 0, 0f);
    }

    // Blood 애니메이션 끝에서 호출
    public void OnBloodEffectFinished()
    {
        StartCoroutine(FadeOutBlood());
    }

    private IEnumerator FadeOutBlood()
    {
        yield return FadeSprite(bloodSR, 1f, 0f, 0.8f);
        bloodEffect.SetActive(false);
        PlayAyaDeath();
    }

    private void PlayAyaDeath()
    {
        ayaDeath.SetActive(true);
        ayaSR.color = new Color(1, 1, 1, 1);
        ayaAnimator.Play(0, 0, 0f);
    }

    // Aya 죽음 애니메이션 끝에서 호출
    public void OnAyaDeathAnimationFinished()
    {
        StartCoroutine(FadeOutAya());
    }

    private IEnumerator FadeOutAya()
    {
        yield return FadeSprite(ayaSR, 1f, 0f, 1.2f);
        ShowGameOverText();
    }

    private void ShowGameOverText()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextSequence());
    }

    private IEnumerator GameOverTextSequence()
    {
        yield return FadeText(gameOverText, 0f, 1f, 1.0f);
        yield return new WaitForSeconds(0.6f);
        yield return PlayEndMenuSequence();
    }

    private IEnumerator PlayEndMenuSequence()
    {
        Vector2 retryOrigin = retryText.anchoredPosition;
        Vector2 quitOrigin = quitText.anchoredPosition;

        retryText.anchoredPosition = retryOrigin - Vector2.up * menuMoveDistance;
        quitText.anchoredPosition = quitOrigin - Vector2.up * menuMoveDistance;

        retryText.gameObject.SetActive(true);
        quitText.gameObject.SetActive(true);

        retryText.DOAnchorPos(retryOrigin, menuMoveDuration)
            .SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(menuInterval);

        quitText.DOAnchorPos(quitOrigin, menuMoveDuration)
            .SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(menuMoveDuration);

        selectionBar.gameObject.SetActive(true);
        selectionBar.position = retryText.position;

        currentIndex = 0;
        UpdateSelectionImmediate();

        canInput = true;
    }

    private void UpdateSelection()
    {
        RectTransform target =
            currentIndex == 0 ? retryText : quitText;

        selectionBar
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
        canInput = false;

        if (currentIndex == 0)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator FadeSprite(SpriteRenderer sr, float from, float to, float time)
    {
        float t = 0f;
        Color c = sr.color;

        while (t < time)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / time);
            sr.color = c;
            yield return null;
        }

        c.a = to;
        sr.color = c;
    }

    private IEnumerator FadeText(TMP_Text text, float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        text.alpha = to;
    }
}
