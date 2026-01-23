using UnityEngine;
using System.Collections;

/// <summary>
/// 전역 사운드 관리 매니저
/// - Master / BGM / Game SFX / UI SFX 볼륨 관리
/// - BGM 페이드 아웃 기능 제공 (씬 전환 제어는 외부 책임)
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public enum SFXType
    {
        Game,
        UI
    }

    [Header("Volume (0~1)")]
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float bgmVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float gameSFXVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float uiSFXVolume = 1f;

    public float MasterVolume => masterVolume;
    public float BGMVolume => bgmVolume;

    [Header("SFX Pool")]
    [SerializeField] private string sfxPoolId = "SFX_Prefab";

    [Header("BGM Fade")]
    [SerializeField] private float bgmFadeOutDuration = 0.8f;

    private AudioSource bgmSource;
    private Coroutine bgmFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitBGMSource();
    }

    private void InitBGMSource()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        ApplyBGMVolume();
    }

    /* =======================
     * Volume Control
     * ======================= */

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyBGMVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        ApplyBGMVolume();
    }

    public void SetGameSFXVolume(float value)
    {
        gameSFXVolume = Mathf.Clamp01(value);
    }

    public void SetUISFXVolume(float value)
    {
        uiSFXVolume = Mathf.Clamp01(value);
    }

    private void ApplyBGMVolume()
    {
        if (bgmSource != null)
            bgmSource.volume = masterVolume * bgmVolume;
    }

    /* =======================
     * BGM Control
     * ======================= */

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        ApplyBGMVolume();
        bgmSource.Play();
    }

    /// <summary>
    /// BGM 페이드 아웃 후 정지
    /// (씬 전환 전에 호출됨)
    /// </summary>
    public Coroutine FadeOutAndStopBGM(MonoBehaviour owner)
    {
        if (bgmSource == null || !bgmSource.isPlaying)
            return null;

        if (bgmFadeCoroutine != null)
            owner.StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = owner.StartCoroutine(FadeOutBGMCoroutine());
        return bgmFadeCoroutine;
    }

    private IEnumerator FadeOutBGMCoroutine()
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < bgmFadeOutDuration)
        {
            time += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / bgmFadeOutDuration);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();

        // 볼륨 값 복구 (다음 BGM 대비)
        ApplyBGMVolume();

        bgmFadeCoroutine = null;
    }

    /* =======================
     * SFX
     * ======================= */

    public void PlayUISFX(AudioClip clip, float volumeScale = 1f)
    {
        PlaySFXInternal(Vector3.zero, clip, SFXType.UI, volumeScale);
    }

    public void PlayGameSFXAt(Vector3 position, AudioClip clip, float volumeScale = 1f)
    {
        PlaySFXInternal(position, clip, SFXType.Game, volumeScale);
    }

    private void PlaySFXInternal(Vector3 position, AudioClip clip, SFXType type, float volumeScale)
    {
        if (clip == null || PoolManager.Instance == null)
            return;

        GameObject obj = PoolManager.Instance.Spawn(sfxPoolId);
        if (obj == null)
            return;

        var emitter = obj.GetComponent<SoundEmitter>();
        if (emitter == null)
        {
            PoolManager.Instance.Despawn(obj);
            return;
        }

        emitter.transform.position = position;
        emitter.Play(clip, GetFinalSFXVolume(type) * volumeScale);
    }

    public float GetFinalSFXVolume(SFXType type)
    {
        float sfx = (type == SFXType.UI) ? uiSFXVolume : gameSFXVolume;
        return masterVolume * sfx;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        ApplyBGMVolume();
    }
#endif
}
