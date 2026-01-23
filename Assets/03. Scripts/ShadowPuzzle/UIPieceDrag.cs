using UnityEngine;
using UnityEngine.EventSystems;

public class UIPieceDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 originalAnchoredPos;
    private Vector2 dragOffset;

    private PuzzleBoard board;
    private PuzzlePiece piece;

    [Header("SFX")]
    [SerializeField] private AudioClip dropSFX; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        piece = GetComponent<PuzzlePiece>();
        board = FindObjectOfType<PuzzleBoard>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalAnchoredPos = rectTransform.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out dragOffset
        );

        board.ClearOccupation(piece);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        rectTransform.anchoredPosition = localPoint - dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 pieceLocalPos = rectTransform.anchoredPosition;

        Vector2Int cell = board.FindNearestValidCell(piece, pieceLocalPos);

        if (cell.x >= 0)
        {
            rectTransform.anchoredPosition =
                board.CellToLocalPos(cell, piece.size);

            board.Occupy(piece, cell);
        }
        else
        {
            rectTransform.anchoredPosition = originalAnchoredPos;
        }

        if (dropSFX != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Camera.main.transform.position,
                dropSFX
            );
        }

        FindObjectOfType<PuzzleAnswerChecker>()?.CheckAnswer();
        Debug.Log($"{piece.name} CellPos = {piece.CellPos}");
    }
}
