using UnityEngine;

public class ImagePuzzleSwapManager : MonoBehaviour
{
    public static ImagePuzzleSwapManager Instance;

    private ImagePuzzlePiece firstSelected;

    private void Awake()
    {
        Instance = this;
    }

    public void OnPieceClicked(ImagePuzzlePiece piece)
    {
        // 첫 번째 클릭
        if (firstSelected == null)
        {
            firstSelected = piece;
            firstSelected.SetSelected(true);
            return;
        }

        // 같은 거 두 번 클릭 → 취소
        if (firstSelected == piece)
        {
            firstSelected.SetSelected(false);
            firstSelected = null;
            return;
        }

        // 두 번째 클릭 → 스왑
        firstSelected.SetSelected(false);
        Swap(firstSelected, piece);
        firstSelected = null;
    }


    private void Swap(ImagePuzzlePiece a, ImagePuzzlePiece b)
    {
        Transform slotA = a.transform.parent;
        Transform slotB = b.transform.parent;

        a.transform.SetParent(slotB, false);
        b.transform.SetParent(slotA, false);

        // 스왑 후 정답 검사
        FindObjectOfType<ImagePuzzleAnswerChecker>().CheckAnswer();
    }

}
