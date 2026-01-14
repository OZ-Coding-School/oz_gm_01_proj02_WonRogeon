using UnityEngine;
using TMPro;

/// <summary>
/// 문 위에 표시되는
/// Zone 이름 미리보기 UI
/// </summary>
public class ZoneNamePreviewUI : MonoBehaviour
{
    public static ZoneNamePreviewUI Instance;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f);

    private Transform followTarget;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position + offset;
        }
    }

    public void Show(string zoneName, Transform target)
    {
        nameText.text = zoneName;
        followTarget = target;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        followTarget = null;
        gameObject.SetActive(false);
    }
}
