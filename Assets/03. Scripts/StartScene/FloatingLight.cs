using UnityEngine;
using UnityEngine.UI;

public class FloatingLight : MonoBehaviour
{
    private enum SizeType { Small, Medium, Large }

    [Header("Move")]
    [SerializeField] float baseSpeed = 120f;

    [Header("Scale")]
    [SerializeField] float smallScale = 0.6f;
    [SerializeField] float mediumScale = 1.0f;
    [SerializeField] float largeScale = 1.4f;

    private RectTransform rt;
    private RectTransform canvasRect;
    private Image image;

    private Vector2 moveDir;
    private float moveSpeed;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        InitRandom();
    }

    private void InitRandom()
    {
        SizeType size = (SizeType)Random.Range(0, 3);

        float angle;
        float scale;

        switch (size)
        {
            case SizeType.Small:
                angle = 20f;
                scale = smallScale;
                moveSpeed = baseSpeed * 0.8f;
                break;

            case SizeType.Medium:
                angle = 45f;
                scale = mediumScale;
                moveSpeed = baseSpeed;
                break;

            default:
                angle = 75f;
                scale = largeScale;
                moveSpeed = baseSpeed * 1.2f;
                break;
        }

        rt.localScale = Vector3.one * scale;

        moveDir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;

        var c = image.color;
        c.a = 1f;
        image.color = c;
    }

    private void Update()
    {
        rt.anchoredPosition += moveDir * moveSpeed * Time.unscaledDeltaTime;

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(null, rt.position),
            null,
            out canvasPos
        );

        // 1024 x 768 ±âÁØ
        const float OUT_X = 512f;
        const float OUT_Y = 384f;

        if (canvasPos.x > OUT_X || canvasPos.y > OUT_Y)
        {
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
}
