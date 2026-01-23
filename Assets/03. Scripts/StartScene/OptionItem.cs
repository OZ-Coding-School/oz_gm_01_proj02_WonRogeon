using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    public enum OptionType
    {
        Master,
        BGM,
        UI,
        Game
    }

    [Header("Type")]
    [SerializeField] private OptionType optionType;

    [Header("Text UI")]
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text valueText;

    [Header("Volume Bar")]
    [SerializeField] private RectTransform whiteBar; // 배경 + 테두리 역할
    [SerializeField] private RectTransform blackBar; // 줄어드는 영역

    [Header("Bar Images (Color Invert)")]
    [SerializeField] private Image whiteBarImage;
    [SerializeField] private Image blackBarImage;

    private const float STEP = 0.05f; // 5%

    private float blackBarMaxWidth;

    private void Awake()
    {
        // 캐싱
        blackBarMaxWidth = blackBar.rect.width;
    }

    /* =========================
     * Public
     * ========================= */

    public void Refresh()
    {
        float volume = GetVolume();
        valueText.text = $"{Mathf.RoundToInt(volume * 100f)}%";
        UpdateBar(volume);
    }

    public void ChangeVolume(int dir)
    {
        float volume = GetVolume();
        volume = Mathf.Clamp01(volume + STEP * dir);
        SetVolume(volume);
        Refresh();
    }

    public void SetSelected(bool selected)
    {
        // 텍스트 색 반전
        labelText.color = selected ? Color.black : Color.white;
        valueText.color = selected ? Color.black : Color.white;

        // 바 색 반전
        if (whiteBarImage != null && blackBarImage != null)
        {
            whiteBarImage.color = selected ? Color.black : Color.white;
            blackBarImage.color = selected ? Color.white : Color.black;
        }
    }

    /* =========================
     * Internal
     * ========================= */

    private float GetVolume()
    {
        var sm = SoundManager.Instance;

        switch (optionType)
        {
            case OptionType.Master:
                return sm.MasterVolume;

            case OptionType.BGM:
                return sm.BGMVolume;

            case OptionType.UI:
                return GetPrivateUISFXVolume(sm);

            case OptionType.Game:
                return GetPrivateGameSFXVolume(sm);
        }

        return 1f;
    }

    private void SetVolume(float value)
    {
        var sm = SoundManager.Instance;

        switch (optionType)
        {
            case OptionType.Master:
                sm.SetMasterVolume(value);
                break;

            case OptionType.BGM:
                sm.SetBGMVolume(value);
                break;

            case OptionType.UI:
                sm.SetUISFXVolume(value);
                break;

            case OptionType.Game:
                sm.SetGameSFXVolume(value);
                break;
        }
    }

    private void UpdateBar(float volume)
    {
        float ratio = 1f - volume;
        float width = blackBarMaxWidth * ratio;

        blackBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            width
        );
    }

    /* =========================
     * SoundManager 보조
     * ========================= */

    private float GetPrivateUISFXVolume(SoundManager sm)
    {
        if (sm.MasterVolume <= 0f)
            return 0f;

        return sm.GetFinalSFXVolume(SoundManager.SFXType.UI) / sm.MasterVolume;
    }

    private float GetPrivateGameSFXVolume(SoundManager sm)
    {
        if (sm.MasterVolume <= 0f)
            return 0f;

        return sm.GetFinalSFXVolume(SoundManager.SFXType.Game) / sm.MasterVolume;
    }
}
