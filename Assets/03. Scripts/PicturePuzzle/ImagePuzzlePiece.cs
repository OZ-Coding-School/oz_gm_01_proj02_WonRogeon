using UnityEngine;
using UnityEngine.EventSystems;

public class ImagePuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject selectionOutline;

    public Transform CorrectSlot { get; private set; }
    public void SetCorrectSlot(Transform slot)
    {
        CorrectSlot = slot;
    }

    private void Awake()
    {
        // 시작 시 선택 테두리 OFF
        if (selectionOutline != null)
            selectionOutline.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ImagePuzzleSwapManager.Instance.OnPieceClicked(this);
    }

    public void SetSelected(bool selected)
    {
        if (selectionOutline != null)
            selectionOutline.SetActive(selected);
    }
}
