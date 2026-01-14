using UnityEngine;

public class PuzzleInteract : MonoBehaviour, IInteractable
{
    [Header("Puzzle UI")]
    [SerializeField] private GameObject puzzlePanel;

    private bool isOpened;

    private void Update()
    {
        if (!isOpened)
            return;

        // 퍼즐 열려 있는 동안 ESC로 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }
    public void Interact()
    {
        // 이미 퍼즐이 열려 있으면 무시
        if (isOpened)
            return;

        OpenPuzzle();
    }

    private void OpenPuzzle()
    {
        isOpened = true;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        // 입력 잠금 & 시간 정지
        Time.timeScale = 0f;

        // 플레이어 상호작용 잠금
        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = false;
    }

    /// <summary>
    /// 퍼즐 패널에서 호출 (닫기 버튼 or 퍼즐 완료 시)
    /// </summary>
    public void ClosePuzzle()
    {
        isOpened = false;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        Time.timeScale = 1f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;
    }
}
