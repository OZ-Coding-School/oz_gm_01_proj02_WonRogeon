using UnityEngine;
using TMPro;

public class SaveConfirmUI : MonoBehaviour
{
    public static SaveConfirmUI Instance;

    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Options")]
    [SerializeField] private TMP_Text[] optionTexts; // 0: Yes, 1: No
    [SerializeField] private RectTransform selectionBar;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.black;

    private int currentIndex;
    private bool isOpen;
    private int slotIndex;

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
            Cancel();
            return;
        }

        HandleMoveInput();
        HandleSubmitInput();
    }

    public void Open(int slotIndex)
    {
        if (isOpen)
            return;

        this.slotIndex = slotIndex;
        isOpen = true;
        currentIndex = 0;

        root.SetActive(true);
        UpdateVisual();
    }

    private void Close()
    {
        isOpen = false;
        root.SetActive(false);
    }

    private void Cancel()
    {
        Close();
        SaveSlotMenuUI.Instance.ResumeFromConfirm();
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
            (currentIndex + dir + optionTexts.Length) % optionTexts.Length;

        UpdateVisual();
    }

    private void HandleSubmitInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (currentIndex == 0)
        {
            // YES : 아직 아무 동작도 하지 않음
            return;
        }
        else
        {
            // NO
            Cancel();
        }
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].color =
                (i == currentIndex) ? selectedColor : normalColor;
        }

        selectionBar.position =
            new Vector3(
                selectionBar.position.x,
                optionTexts[currentIndex].transform.position.y,
                selectionBar.position.z
            );
    }
}
