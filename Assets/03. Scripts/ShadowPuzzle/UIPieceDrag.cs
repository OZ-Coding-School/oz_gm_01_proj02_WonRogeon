using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 퍼즐 피스 드래그 전용 스크립트
/// - UI Image 기반 드래그
/// - 화면 밖으로 드롭 시 원래 위치로 복귀
/// </summary>
public class UIPieceDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 originalAnchoredPos;
    private Vector2 dragOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 위치 저장
        originalAnchoredPos = rectTransform.anchoredPosition;

        // 마우스와 피스 중심 간 오프셋 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out dragOffset
        );
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
        if (IsOutOfScreen())
        {
            // 화면 밖으로 나가면 원래 위치로 복귀
            rectTransform.anchoredPosition = originalAnchoredPos;
        }
    }

    /// <summary>
    /// 피스가 화면 영역을 벗어났는지 체크
    /// </summary>
    private bool IsOutOfScreen()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, corners[i]);

            if (screenPos.x < 0 || screenPos.x > Screen.width ||
                screenPos.y < 0 || screenPos.y > Screen.height)
            {
                return true;
            }
        }

        return false;
    }
}
