using UnityEngine;

public class OptionPanelController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private OptionItem[] options;

    [Header("Selection Bar")]
    [SerializeField] private RectTransform selectionBar;

    [Header("Selection Offset")]
    [SerializeField] private float selectionYOffset = -15f;

    [Header("UI SFX")]
    [SerializeField] private AudioClip moveSFX;



    private int currentIndex;
    private bool isOpen;

    private void OnEnable()
    {
        isOpen = true;
        currentIndex = 0;

        RefreshAll();
        UpdateSelection(true);
    }

    private void Update()
    {
        if (!isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + options.Length) % options.Length;
            UpdateSelection(false);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % options.Length;
            UpdateSelection(false);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            options[currentIndex].ChangeVolume(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            options[currentIndex].ChangeVolume(+1);
        }
    }

    private void UpdateSelection(bool immediate)
    {
        for (int i = 0; i < options.Length; i++)
            options[i].SetSelected(i == currentIndex);

        MoveSelectionBar(immediate);
    }

    private void MoveSelectionBar(bool immediate)
    {
        if (selectionBar == null)
            return;

        RectTransform target = options[currentIndex].GetComponent<RectTransform>();

        Vector3 targetPos = new Vector3(
            selectionBar.position.x,
            target.position.y + selectionYOffset,
            selectionBar.position.z
        );

        selectionBar.position = targetPos;

        // 셀렉션바 이동 사운드
        if (!immediate && moveSFX != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISFX(moveSFX);
        }
    }



    private void RefreshAll()
    {
        foreach (var opt in options)
            opt.Refresh();
    }

    private void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);

        FindObjectOfType<StartMenuController>()?.OnOptionClosed();
    }

}
