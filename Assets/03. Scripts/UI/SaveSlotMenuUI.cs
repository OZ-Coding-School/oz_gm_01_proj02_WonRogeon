using UnityEngine;
using TMPro;

public class SaveSlotMenuUI : MonoBehaviour
{
    public static SaveSlotMenuUI Instance;

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
    private bool inputLocked;

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
        if (SaveConfirmUI.Instance != null && SaveConfirmUI.Instance.IsOpen)
            return;

        if (!isOpen || inputLocked)
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
        inputLocked = false;
        currentIndex = 0;

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

    public void ResumeFromConfirm()
    {
        inputLocked = false;
        UpdateVisual();
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
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        PlayUISFX(submitSFX);
        inputLocked = true;
        SaveConfirmUI.Instance.Open(currentIndex);
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

    public void RefreshSlot(int index)
    {
        SaveData data = SaveManager.Instance.Load(index);
        if (data == null)
            return;

        string time = System.TimeSpan
            .FromSeconds(data.playTimeSeconds)
            .ToString(@"hh\:mm\:ss");

        slotTexts[index].text =
            $"DATA{index + 1:D2}   {data.floorAndRoom}   {time}";
    }

    private void LoadAllSlots()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            SaveData data = SaveManager.Instance.Load(i);
            if (data == null)
                continue;

            string time = System.TimeSpan
                .FromSeconds(data.playTimeSeconds)
                .ToString(@"hh\:mm\:ss");

            slotTexts[i].text =
                $"DATA{i + 1:D2}   {data.floorAndRoom}   {time}";
        }
    }

    private void PlayUISFX(AudioClip clip)
    {
        if (clip == null || SoundManager.Instance == null)
            return;

        SoundManager.Instance.PlayUISFX(clip);
    }
}
