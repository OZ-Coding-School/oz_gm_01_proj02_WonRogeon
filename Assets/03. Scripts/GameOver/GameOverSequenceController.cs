// GameOverSequenceController.cs (발췌)
using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverSequenceController : MonoBehaviour
{
    public static GameOverSequenceController Instance;

    [Header("References")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject ayaDeath;
    [SerializeField] private TMP_Text gameOverText;

    private Animator bloodAnimator;
    private Animator ayaAnimator;
    private SpriteRenderer bloodSR;
    private SpriteRenderer ayaSR;

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

        PlayBloodEffect();
    }

    private void PlayBloodEffect()
    {
        bloodEffect.SetActive(true);
        bloodSR.color = new Color(1, 1, 1, 1);
        bloodAnimator.Play(0, 0, 0f);
    }

    // Blood 애니메이션 끝에서 호출됨
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

    // Aya 죽음 애니메이션 끝에서 호출됨
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
        StartCoroutine(FadeText(gameOverText, 0f, 1f, 1.0f));
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
