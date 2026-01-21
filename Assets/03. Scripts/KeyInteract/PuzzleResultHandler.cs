using UnityEngine;

public class PuzzleResultHandler : MonoBehaviour
{
    [SerializeField] private GameObject secretWallTilemap;

    private void OnEnable()
    {
        PuzzleAnswerChecker.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDisable()
    {
        PuzzleAnswerChecker.OnPuzzleSolved -= HandlePuzzleSolved;
    }

    private void HandlePuzzleSolved()
    {
        if (secretWallTilemap != null)
            secretWallTilemap.SetActive(false);
    }
}
