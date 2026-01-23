using UnityEngine;
using TMPro;
using System.Collections;

public class LoadSlotMenuUI : MonoBehaviour
{
    public static LoadSlotMenuUI Instance;

    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Slots")]
    [SerializeField] private TMP_Text[] slotTexts;
    [SerializeField] private RectTransform selectionBar;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.black;

    [Header("UI SFX")]
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] private AudioClip submitSFX;
    [SerializeField] private AudioClip backSFX;

    private int currentIndex;
    private bool isOpen;
    private bool ignoreSubmitOnce;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        root.SetActive(false);
    }

    private void Update()
    {
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
            return;

        if (!isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayUISFX(backSFX);
            Close();
            SaveMenuUI.Instance.ReopenFromSlot();
            return;
        }

        HandleMoveInput();
        HandleSubmitInput();
    }

    public void Open()
    {
        if (isOpen)
            return;

        isOpen = true;
        currentIndex = 0;
        ignoreSubmitOnce = true;

        root.SetActive(true);
        Time.timeScale = 0f;

        LoadAllSlots();
        UpdateVisual();
    }

    public void Close()
    {
        isOpen = false;
        root.SetActive(false);
    }

    private void HandleMoveInput()
    {
        int dir = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            dir = -1;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            dir = 1;

        if (dir == 0)
            return;

        currentIndex =
            (currentIndex + dir + slotTexts.Length) % slotTexts.Length;

        PlayUISFX(moveSFX);
        UpdateVisual();
    }

    private void HandleSubmitInput()
    {
        if (ignoreSubmitOnce)
        {
            ignoreSubmitOnce = false;
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        SaveData data = SaveManager.Instance.Load(currentIndex);
        if (data == null)
            return;

        PlayUISFX(submitSFX);
        LoadGame(data);
    }

    private void LoadGame(SaveData data)
    {
        StartCoroutine(LoadRoutine(data));
    }

    private IEnumerator LoadRoutine(SaveData data)
    {
        isOpen = false;
        Time.timeScale = 1f;

        yield return FadeController.Instance.FadeOut();

        // 플레이 시간
        PlayTimeTracker.Instance.SetPlayTime(data.playTimeSeconds);

        // 열쇠 복구
        KeyInventory.Instance.RestoreKeys(data.ownedKeys);

        // Zone 이동
        ZoneManager.Instance.LoadZone(data.floor, data.zoneName);

        yield return null;

        foreach (var key in FindObjectsOfType<KeyPickupInteract>(true))
        {
            key.SyncWithInventory();
        }

        root.SetActive(false);

        yield return FadeController.Instance.FadeIn();
    }

    private void LoadAllSlots()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            SaveData data = SaveManager.Instance.Load(i);
            if (data == null)
            {
                slotTexts[i].text = $"DATA{i + 1:D2}   NO DATA";
                continue;
            }

            string time = System.TimeSpan
                .FromSeconds(data.playTimeSeconds)
                .ToString(@"hh\:mm\:ss");

            slotTexts[i].text =
                $"DATA{i + 1:D2}   {data.floorAndRoom}   {time}";
        }
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            slotTexts[i].color =
                (i == currentIndex) ? selectedColor : normalColor;
        }

        selectionBar.position =
            new Vector3(
                selectionBar.position.x,
                slotTexts[currentIndex].transform.position.y,
                selectionBar.position.z
            );
    }

    private void PlayUISFX(AudioClip clip)
    {
        if (clip == null || SoundManager.Instance == null)
            return;

        SoundManager.Instance.PlayUISFX(clip);
    }
}
