using UnityEngine;
using TMPro;

public class SaveMenuUI : MonoBehaviour
{
    public static SaveMenuUI Instance;

    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text[] menuTexts;

    [Header("Color")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color selectedColor = new Color(1f, 1f, 1f, 1f);

    private int currentIndex;
    private bool isOpen;
    private bool ignoreEscapeOnce;   

    public bool IsOpen => isOpen;

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
        if (!isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ignoreEscapeOnce)
            {
                ignoreEscapeOnce = false; // 이 프레임 ESC 무시
                return;
            }

            Close(true);
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
        ignoreEscapeOnce = false;

        root.SetActive(true);
        Time.timeScale = 0f;

        UpdateVisual();
    }

    // SaveSlotMenuUI에서 돌아올 때 호출
    public void ReopenFromSlot()
    {
        isOpen = true;
        currentIndex = 0;
        ignoreEscapeOnce = true;   // 여기서만 ESC 1회 차단

        root.SetActive(true);
        UpdateVisual();
    }

    public void Close(bool restoreTimeScale)
    {
        isOpen = false;
        root.SetActive(false);

        if (restoreTimeScale)
            Time.timeScale = 1f;
    }

    private void HandleMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + menuTexts.Length) % menuTexts.Length;
            UpdateVisual();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % menuTexts.Length;
            UpdateVisual();
        }
    }

    private void HandleSubmitInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (currentIndex == 0) // SAVE
        {
            Close(false);
            SaveSlotMenuUI.Instance.Open();
        }
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < menuTexts.Length; i++)
        {
            menuTexts[i].color =
                (i == currentIndex) ? selectedColor : normalColor;
        }
    }
}
