using UnityEngine;

public class ImagePuzzleInteract : MonoBehaviour, IInteractable
{
    [Header("Puzzle UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private PuzzleImageRandomizer randomizer;

    private bool isOpened;

    public void Interact()
    {
        if (isOpened)
            return;

        OpenPuzzle();
    }

    private void OpenPuzzle()
    {
        isOpened = true;

        // 퍼즐 랜덤 배치
        if (randomizer != null)
            randomizer.Randomize();

        // 퍼즐 패널 열기
        puzzlePanel.SetActive(true);

        // 게임 정지
        Time.timeScale = 0f;

        // 플레이어 상호작용 비활성화
        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = false;
    }

    // 퍼즐 종료 시 외부에서 호출용
    public void ClosePuzzle()
    {
        if (!isOpened)
            return;

        isOpened = false;

        puzzlePanel.SetActive(false);

        Time.timeScale = 1f;

        var playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;
    }
}
