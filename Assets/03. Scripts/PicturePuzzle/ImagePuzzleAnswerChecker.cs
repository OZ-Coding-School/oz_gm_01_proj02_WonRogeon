using UnityEngine;

public class ImagePuzzleAnswerChecker : MonoBehaviour
{
    [SerializeField] private ImagePuzzlePanelController panelController;

    public void CheckAnswer()
    {
        var pieces = GetComponentsInChildren<ImagePuzzlePiece>();

        foreach (var piece in pieces)
        {
            if (piece.transform.parent != piece.CorrectSlot)
                return;
        }

        // 전부 맞음
        panelController.OnPuzzleSolved();
    }
}
