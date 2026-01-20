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

    private int currentIndex;
    private bool isOpen;

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
            Close();
            SaveMenuUI.Instance.Open();
            return;
        }

        HandleMoveInput();
    }

    public void Open()
    {
        if (isOpen)
            return;

        isOpen = true;
        currentIndex = 0;

        root.SetActive(true);
        Time.timeScale = 0f;

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

        UpdateVisual();
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
}
